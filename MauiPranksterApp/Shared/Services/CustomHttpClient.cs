using System;
using System.Net.Http;

namespace MauiPranksterApp.Shared.Services
{
    public class CustomHttpClient
    {
        public HttpClient c { get; private set; }

        public CustomHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            httpClient.BaseAddress = new Uri("https://api.pranksterapp.com/");
            httpClient.DefaultRequestHeaders.Add("Prankster-Platform","ios-app");

            c = httpClient;
        }
    }
}
