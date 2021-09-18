using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Model.Services;
using Shared.ApiErrors;
using Model.Extensions;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileServices _profileServices;
        private IConfiguration _configuration { get; }

        public ProfileController(IProfileServices profileServices, IConfiguration configuration)
        {
            _profileServices = profileServices;
            _configuration = configuration;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.GetId();
            var userProfile = await _profileServices.GetProfile(userId);

            return userProfile == null
                ? Conflict(new ConflictError("profile_get_error"))
                : Ok(userProfile);
        }

        /*
        [HttpPut("birth")]
        public async Task<IActionResult> UpdateBirth(UpdateProfileBirth newBirth)
        {
            // Check if +18
            if (newBirth.Birth == null)
                return Conflict(new ConflictError("birth_empty"));

            var notMajorAje = DateTime.Compare(newBirth.Birth.Value.AddYears(18), DateTime.Now) == 1;

            if (newBirth.Birth.Value.Year < DateTime.Now.Year - 100 || notMajorAje)
                return Conflict(new ConflictError("not_major_age"));

            var userId = User.GetId();
            var result = await _profileServices.UpdateBirth(userId, newBirth.Birth);

            if (result == false)
                return Conflict(new ConflictError("birth_update_error"));

            return Ok();
        }

        [HttpPut("country")]
        public async Task<IActionResult> UpdateCountry(UpdateProfileCountry newCountry)
        {
            if (string.IsNullOrWhiteSpace(newCountry.Country))
                return Conflict(new ConflictError("country_empty"));

            if (!Enum.IsDefined(typeof(BlackList.Countries), newCountry.Country))
                return Conflict(new ConflictError("country_not_allowed"));

            var userId = User.GetId();
            var result = await _profileServices.UpdateCountry(userId, newCountry.Country);

            if (result == false)
                return Conflict(new ConflictError("country_update_error"));

            return Ok();
        }

        [HttpPut("username")]
        public async Task<IActionResult> UpdateUsername(UpdateProfileUsername newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername.Username) || newUsername.Username.Length < 4)
                return Conflict(new ConflictError("username_too_small"));

            if (newUsername.Username.Length > 15)
                return Conflict(new ConflictError("username_too_big"));

            // Check username regex
            if (!newUsername.Username.All(char.IsLetterOrDigit))
                return Conflict(new ConflictError("username_symbols"));

            // Check if username is blacklisted
            if (BlackList.Names.Any(name => name == newUsername.Username))
                return Conflict(new ConflictError("username_blacklisted"));

            bool usernameExists = await _userServices.UsernameExistsAsync(newUsername.Username);
            if (usernameExists)
                return Conflict(new ConflictError("username_already_exists"));

            var userId = User.GetId();
            // var dummy = HttpContext.Session.Id
            var result = await _profileServices.UpdateUsername(userId, newUsername.Username);

            if (result == false)
                return Conflict(new ConflictError("username_update_error"));

            return Ok();
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(UpdateProfileMail newMail)
        {
            if (string.IsNullOrEmpty(newMail.Email) || newMail.Email.Length > 100 ||
                !CheckValidEmail.Validate(newMail.Email))
                return Conflict(new ConflictError("email_invalid"));

            // Check email if registered // so sleepy, did not notice :/
            var emailRegistered = await _userServices.EmailExistsAsync(newMail.Email);
            if (emailRegistered)
                return Conflict(new ConflictError("email_already_exists"));

            var userId = User.GetId();
            var result = await _profileServices.UpdateEmail(userId, newMail.Email);

            if (result == false)
                return Conflict(new ConflictError("email_update_error"));

            return Ok();
        }
        */
    }
}