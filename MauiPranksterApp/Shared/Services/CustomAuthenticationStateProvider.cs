using Blazored.LocalStorage;
using MauiPranksterApp.Native.iOS;
using Microsoft.AspNetCore.Components.Authorization;
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

namespace MauiPranksterApp.Shared.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
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
            var storedUser = await _localStorage.GetItemAsync<UserData>("user");


            if (storedUser == null || string.IsNullOrEmpty(storedUser.Token))
            {
                var platform = global::Shared.Enums.Platform.iOS;

                if (platform == global::Shared.Enums.Platform.iOS)
                {
                    string identifier = KeyChain.ValueForKey("simpleidentifier");
                    string pin = KeyChain.ValueForKey("pin");
                    if (!int.TryParse(pin, out int checkedPin))
                        pin = null;

                    if (!string.IsNullOrEmpty(identifier) && !string.IsNullOrEmpty(pin))
                    {
                        var response = await _httpClient.PostAsJsonAsync("account/auth", await BuildUserLoginDtoAsync(identifier, checkedPin));
                        if (!response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.NotImplemented || response.StatusCode ==HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.GatewayTimeout)
                                    throw new Exception("Server not operational");

                            var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
                            string[] acceptedErrorCodesForReset = { "identifier_unallowed", "auth_account_issue" };

                            if (acceptedErrorCodesForReset.Any(m => m == apiError?.Message))
                                return AnonymousAuthenticationState();

                            Console.WriteLine(apiError);
                            throw new Exception("Unexpected issue: Visit pranksterapp.com to get help");
                        }

                        var fetchedUser = await response.Content.ReadFromJsonAsync<UserData>();
                        if (fetchedUser == null)
                            return AnonymousAuthenticationState();


                        await SetCurrentUserAsync(fetchedUser);
                        storedUser = fetchedUser;
                    }
                    else
                        // any new user should call AAS method here
                        return AnonymousAuthenticationState();

                }
                else
                    throw new NotImplementedException("Platform not available");

            }

            /* 
             * We are gonna check if the token is still valid and save log, if not user will stay in Index (Register)
             * which will try to make auth again with keychain values, if its not there, will be considered as a new signup (finally)
             * user may contact us to see what happened and check if we can give his call ballance back
             */
           
            var authCheckResponse = await _httpClient.PostAsJsonAsync("account/auth/check", await BuildUserLogDtoAsync());
            if (!authCheckResponse.IsSuccessStatusCode)
            {
                if (authCheckResponse.StatusCode == HttpStatusCode.InternalServerError || authCheckResponse.StatusCode == HttpStatusCode.NotImplemented || authCheckResponse.StatusCode == HttpStatusCode.NotFound || authCheckResponse.StatusCode == HttpStatusCode.RequestTimeout || authCheckResponse.StatusCode == HttpStatusCode.GatewayTimeout)
                    throw new Exception("Server not operational");

                var apiError = await authCheckResponse.Content.ReadFromJsonAsync<ApiError>();
                string[] acceptedErrorCodesForReset = { "unauthorized", "no_permission"};

                if (acceptedErrorCodesForReset.Any(m => m == apiError?.Message))
				{
                    await _localStorage.RemoveItemAsync("user");
                    return AnonymousAuthenticationState();
                }

                Console.WriteLine(apiError);
                throw new Exception("Unexpected issue: Visit pranksterapp.com to get help");
            }

            // If everything went well, user is now authenticated into the app

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", storedUser.Token);

            // TODO: Para I18N, establecer el lenguaje preferido contenido en UserData

            return CreateAuthState(storedUser);
        }

        public async Task SetCurrentUserAsync(UserData userData)
        {
            var authState = CreateAuthState(userData);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            await _localStorage.SetItemAsync("user", userData);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userData.Token);
        }

        public async Task ClearCurrentUserAsync()
        {
            var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var authStateTask = Task.FromResult(new AuthenticationState(anonymousPrincipal));
            NotifyAuthenticationStateChanged(authStateTask);

            await _localStorage.RemoveItemAsync("user");
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
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
        private AuthenticationState AnonymousAuthenticationState() => new(new ClaimsPrincipal(new ClaimsIdentity()));


        private static Task<UserLoginDto> BuildUserLoginDtoAsync(string simpleIdentifier = null, int pin = 0) =>
        Task.FromResult(new UserLoginDto()
        {
            ComplexIdentifier = (simpleIdentifier == null && pin == 0) ? IdentifierUtils.CreateComplexIdentifier() : null,
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

        private static void SetKeychain(string simpleIdentifier, string pin)
		{
            KeyChain.SetValueForKey("simpleidentifier", simpleIdentifier);
            KeyChain.SetValueForKey("pin", pin);
		}

 
    }
}
