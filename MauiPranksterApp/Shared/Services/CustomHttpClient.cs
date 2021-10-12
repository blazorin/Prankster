using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MauiPranksterApp.Shared.Services
{
    public class CustomHttpClient
    {
        public HttpClient c { get; private set; }
        public static AuthenticationHeaderValue TokenHeader { get; set; }

        public CustomHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            httpClient.BaseAddress = new Uri("https://api.pranksterapp.com");
            httpClient.DefaultRequestHeaders.Add("Prankster-Platform","ios-app");
            httpClient.DefaultRequestHeaders.Authorization = TokenHeader;

            c = httpClient;
        }
    }
}
