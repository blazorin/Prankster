using Blazored.LocalStorage;
using MauiPranksterApp.Native.iOS;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shared.Dto;
using Shared.Utils;
using Microsoft.Maui.Essentials;
using Shared.ApiErrors;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace MauiPranksterApp.Shared.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly CustomHttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        // Since we're changing RouteViews in every page navigation this is a good workaround.
        // Not discarding a PR should be made about this in the future with a better solution
        private static AuthenticationState storedAuthState;

        public CustomAuthenticationStateProvider(CustomHttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        /// <summary>
        /// Este método se llama automáticamente por AuhtorizeRouteView en la inicialización
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (storedAuthState is not null)
                return storedAuthState;

            var storedUser = await _localStorage.GetItemAsync<UserData>("user");


            if (storedUser == null || string.IsNullOrEmpty(storedUser.Token))
            {
                var platform = global::Shared.Enums.Platform.iOS;

                if (platform == global::Shared.Enums.Platform.iOS)
                {
                    var (state, userData) = await SendAuthenticationRequest(false);
                    if (userData is null)
                        return state;

                    storedUser = userData;
                }
                else
                    throw new NotImplementedException("Platform not available");

            }

            // set token in HttpClient

            SetToken(storedUser.Token);

            /* 
             * We are gonna check if the token is still valid and save log, if not user will stay in Index (Register)
             * which will try to make auth again with keychain values, if its not there, will be considered as a new signup (finally)
             * user may contact us to see what happened and check if we can give his call ballance back
             */

            var userLog = await BuildUserLogDtoAsync();
            string serializedUserLog = JsonSerializer.Serialize(userLog);

            var authCheckResponse = await _httpClient.c.PutAsJsonAsync("account/auth/check", serializedUserLog);
            if (!authCheckResponse.IsSuccessStatusCode)
            {
                if (authCheckResponse.StatusCode == HttpStatusCode.InternalServerError || authCheckResponse.StatusCode == HttpStatusCode.NotImplemented || authCheckResponse.StatusCode == HttpStatusCode.NotFound || authCheckResponse.StatusCode == HttpStatusCode.RequestTimeout || authCheckResponse.StatusCode == HttpStatusCode.GatewayTimeout)
                    throw new Exception("Server not operational");

                var apiError = await authCheckResponse.Content.ReadFromJsonAsync<ApiError>();
                string[] acceptedErrorCodesForReset = { "unauthorized", "no_permission", "auth_check_error" };

                if (acceptedErrorCodesForReset.Any(m => m == apiError?.Message))
				{
                    await _localStorage.RemoveItemAsync("user");
                    RemoveToken();

                    return AnonymousAuthenticationState();
                }

                Console.WriteLine(apiError);
                throw new Exception("Unexpected issue: Visit pranksterapp.com to get help. Error code: PA-02");
            }

            // If everything went well, user is now authenticated into the app

            // TODO: Para I18N, establecer el lenguaje preferido contenido en UserData

            AuthenticationState authState = CreateAuthState(storedUser);
            storedAuthState = authState;

            return authState;
        }

        private async Task SetCurrentUserAsync(UserData userData, bool notFromThisClass)
        {
            if (notFromThisClass)
            {
                var authState = CreateAuthState(userData);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
            }

            await _localStorage.SetItemAsync("user", userData);
            SetToken(userData.Token);
        }

        public async Task ClearCurrentUserAsync()
        {
            var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var authStateTask = Task.FromResult(new AuthenticationState(anonymousPrincipal));
            NotifyAuthenticationStateChanged(authStateTask);

            await _localStorage.RemoveItemAsync("user");
            RemoveToken();
        }

        private static AuthenticationState CreateAuthState(UserData userData)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.OtherPhone, userData.Pin.ToString())
            };
            claims.AddRange(userData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Convert back to <Claim>
            foreach (var (key, value) in userData.Policies)
            {
                claims.Add(new Claim(key, value));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "custom");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }

        // We will show him the Register (Index)
        private static AuthenticationState AnonymousAuthenticationState() => new(new ClaimsPrincipal(new ClaimsIdentity()));


        private static Task<UserLoginDto> BuildUserLoginDtoAsync(string simpleIdentifier = null, int pin = 0) =>
        Task.FromResult(new UserLoginDto()
        {
            ComplexIdentifier = (string.IsNullOrEmpty(simpleIdentifier) && pin is 0) ? IdentifierUtils.CreateComplexIdentifier() : null,
            Identifier = simpleIdentifier,
            Pin = pin,
            LastPlatform = global::Shared.Enums.Platform.iOS,
            DeviceModel = DeviceInfo.Model,
            OSVersion = !string.IsNullOrEmpty(DeviceInfo.VersionString) ? DeviceInfo.VersionString : "1.0"
        });

        private static Task<BasicUserLogDto> BuildUserLogDtoAsync() =>
        Task.FromResult(new BasicUserLogDto()
        {
            LastPlatform = global::Shared.Enums.Platform.iOS,
            DeviceModel = DeviceInfo.Model,
            OSVersion = !string.IsNullOrEmpty(DeviceInfo.VersionString) ? DeviceInfo.VersionString : "1.0"
        });

        public async Task<(AuthenticationState, UserData)> SendAuthenticationRequest(bool canCreate, bool needsNotifyAuthTask = false)
        {
            //string identifier = KeyChain.ValueForKey("simpleidentifier");
            //string pin = KeyChain.ValueForKey("pin");
            string identifier = null;
            string pin = null;

            if (!int.TryParse(pin, out int checkedPin))
                checkedPin = 0;


            if ((!string.IsNullOrEmpty(identifier) && !string.IsNullOrEmpty(pin)) || canCreate)
            {
                var userlogin = await BuildUserLoginDtoAsync(identifier, checkedPin);
                string serializedUserLogin = JsonSerializer.Serialize(userlogin);


                var response = await _httpClient.c.PostAsJsonAsync("account/auth", serializedUserLogin);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.NotImplemented || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
                        throw new Exception("Server not operational");

                    var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
                    string[] acceptedErrorCodesForReset = { "identifier_unallowed", "auth_account_issue" };

                    if (acceptedErrorCodesForReset.Any(m => m == apiError?.Message))
                        return (AnonymousAuthenticationState(), null);

                    Console.WriteLine(apiError);
                    throw new Exception("Unexpected issue: Visit pranksterapp.com to get help. Error code: PA-01");
                }

                var fetchedUser = await response.Content.ReadFromJsonAsync<UserData>();
                if (fetchedUser == null)
                    return (AnonymousAuthenticationState(), null);


                await SetCurrentUserAsync(fetchedUser, needsNotifyAuthTask);

                if (canCreate)
                SetKeychain(fetchedUser.FinalIdentifier, fetchedUser.Pin.ToString());

                return (CreateAuthState(fetchedUser), fetchedUser);
            }
            else
                // any new user should call AAS method here
                return (AnonymousAuthenticationState(), null);
        }

        private void SetToken(string token)
        {
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            _httpClient.c.DefaultRequestHeaders.Authorization = authHeader;
            CustomHttpClient.TokenHeader = authHeader;
        }

        private void RemoveToken()
        {
            _httpClient.c.DefaultRequestHeaders.Remove("Authorization");
            CustomHttpClient.TokenHeader = null;
        }

        private static void SetKeychain(string simpleIdentifier, string pin)
		{   /*
            KeyChain.SetValueForKey("simpleidentifier", simpleIdentifier);
            KeyChain.SetValueForKey("pin", pin);
            */
		}
 
    }
}
