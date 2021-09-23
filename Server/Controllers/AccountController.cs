using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Data;
using Model.Services;
using Shared;
using Shared.ApiErrors;
using Shared.Dto;
using Google.Apis.Auth;
using Model.Enums;
using Shared.Enums;
using Server.Secure;

namespace Server.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private IConfiguration _configuration { get; }

        public AccountController(IUserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login(UserLoginDto credentials)
        {
            /*
             * Previous checks
             */

            #region Login Checks

            if (User?.Identity != null && User.Identity.IsAuthenticated)
                return Forbid();

            bool isLogin = false;
            // sign up attempt
            if (string.IsNullOrEmpty(credentials.Identifier))
            {
                if (credentials.ComplexIdentifier is null || credentials.ComplexIdentifier.Count != 5 || !char.IsLetter(credentials.ComplexIdentifier[4].Last()))
                    return Unauthorized(new UnauthorizedError("broken_identifier"));


                for (int i = 0; i < credentials.ComplexIdentifier.Count - 1;)
                {
                    if (!int.TryParse(credentials.ComplexIdentifier[i], out int _))
                        return Unauthorized(new UnauthorizedError("broken_identifier"));

                    i++;
                }

                for (int i = 0; i < 2;) 
                {
                    if (!int.TryParse(credentials.ComplexIdentifier[4].ElementAt(i).ToString(), out int _))
                        return Unauthorized(new UnauthorizedError("broken_identifier"));

                    i++;
                }

                // convert to simpleIdentifier
                credentials.ComplexIdentifier.ForEach(part => credentials.Identifier += part);

            }
            else // login attempt
            {
                if (credentials.Pin is 0)
                    return Unauthorized(new UnauthorizedError("broken_identifier"));

                if (!char.IsLetter(credentials.Identifier.Last()))
                    return Unauthorized(new UnauthorizedError("broken_identifier"));


                for (int i = 0; i < credentials.Identifier.Length - 1;)
                {
                    if (!int.TryParse(credentials.Identifier.ElementAt(i).ToString(), out int _))
                        return Unauthorized(new UnauthorizedError("broken_identifier"));

                    i++;
                }

                isLogin = true;
            }

            if (credentials.Identifier.Length is > 20 or < 20)
                return Unauthorized(new UnauthorizedError("broken_identifier"));


            // IP is obtained from HttpContext
            if (!string.IsNullOrWhiteSpace(credentials.IPAddress))
                return Unauthorized(new UnauthorizedError("ip_address_error"));

            if (credentials.LastPlatform is Platform.Missing)
                return Unauthorized(new UnauthorizedError("no_platform_provided"));
            if (credentials.LastPlatform is Platform.Android)
                return Unauthorized(new UnauthorizedError("unsupported_platform"));

            if (string.IsNullOrWhiteSpace(credentials.OSVersion) || credentials.OSVersion.Length > 10)
                return Unauthorized(new UnauthorizedError("invalid_os"));



            // get IP from Cloudflare header
            string? ipHeader = HttpContext?.Request.Headers["CF-Connecting-IP"].FirstOrDefault();

            if (!IPAddress.TryParse(ipHeader, out var ip))
                return Unauthorized(new UnauthorizedError("ip_address_invalid"));

            credentials.IPAddress = ip.ToString();

            if (!string.IsNullOrEmpty(credentials.DeviceModel) && credentials.DeviceModel.Length > 40)
                return Unauthorized(new UnauthorizedError("device_model_error"));

            if (!string.IsNullOrEmpty(credentials.Email))
                return Unauthorized(new UnauthorizedError("not_oauth_login_method"));

            if (!await _userServices.IdentifierExistsAsync(credentials.Identifier))
                return isLogin ? Unauthorized(new UnauthorizedError("identifier_unallowed")) : await Register(credentials);
            
         
            if (await _userServices.IsBannedAsync(credentials.Identifier))
                return Unauthorized(new UnauthorizedError("user_violated_tos"));


            var user = await _userServices.GetUserByAuthenticationAsync(credentials, UserLogType.Login);
            if (user == null)
                return Unauthorized(new UnauthorizedError("auth_account_issue"));

            #endregion

            /*
             * If all ok, Continues here
             */

            var userData = HandleGenerateToken(user);
            return Ok(userData);
        }


        public async Task<IActionResult> Register(UserLoginDto newUser)
        {
            /*
             * Previous checks already done!
             */

            var user = await _userServices.AddUserAsync(newUser, UserLogType.SignUp);

            var userData = HandleGenerateToken(user);
            return Ok(userData);
        }

        /* Oauth Web API Calls */

        [HttpPost("oauth/google")]
        public async Task<IActionResult> GoogleOauth(GoogleLoginRequest request)
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
                return Forbid();

            var emailExists = await _userServices.EmailExistsAsync(request.Email);

            if (!emailExists && (string.IsNullOrEmpty(request.Identifier) || request.Pin == 0))
                return Unauthorized(new UnauthorizedError("google_needs_identifier"));

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    });
            }
            catch
            {
                return Unauthorized(new UnauthorizedError("google_invalid_token"));
            }

            UserLoginDto credentials = request;
            credentials.Email = payload.Email; // set email from gapi payload

            var user = await _userServices.HandleOauthAuthenticationAsync(credentials, OauthType.Google);
            if (user == null)
                return Unauthorized(new UnauthorizedError("google_oauth_error"));

            var userData = HandleGenerateToken(user);
            return Ok(userData);
        }

        /*
        [HttpGet("email/{email?}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var result = await _userServices.EmailExistsAsync(email);
            return Ok(result);
        }

        [HttpGet("username/{username?}")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var result = await _userServices.UsernameExistsAsync(username);
            return Ok(result);
        }
        */

        private static UserData HandleGenerateToken(User user)
        {
            // Claims are not supported to be sent in JSON (no default constructor), so I've made this workaround
            var policiesInClaims = new List<Claim>();
            policiesInClaims.AddRange(user.Perms.Select(perm => new Claim(perm.PermKey, perm.PermValue)));

            var policiesInString =
                user.Perms.ToDictionary(perm => perm.PermKey, perm => perm.PermValue);

            var roles = new List<string> { "user" };

            if (user.IsPremium)
                roles.Add("premium");

            var response = new UserData
            {
                Policies = policiesInString,
                Roles = roles,
                Token = GenerateToken(user, roles, policiesInClaims),
                CallBalance = user.CallBalance,
                Language = user.Language,
                FinalIdentifier = user.Identifier,
                Pin = user.Pin
            };

            return response;
        }

        private static string GenerateToken(User user, IEnumerable<string> roles, IEnumerable<Claim> policies)
        {
            var header = new JwtHeader(
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(PrivateKeys.JwtSecretKey)
                    ),
                    SecurityAlgorithms.HmacSha256)
            );

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId),
#pragma warning disable CS8604 // Possible null reference argument.
                string.IsNullOrEmpty(user.Email) ? null : new Claim(ClaimTypes.Email, user.Email)
#pragma warning restore CS8604 // Possible null reference argument.
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(policies);

            var payload = new JwtPayload(
                issuer: "MauiPServer",
                audience: "MauiPClient",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(180)
            );
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}