using System;
using System.Net.Http;
using Model.Services.Http.Internal;

namespace Model.Services.Http
{
    public class Bitsbi
    {
        private readonly HttpClient Client;

        public Bitsbi(HttpClient client)
        {
            client.BaseAddress = new Uri(WorldWideTelephony.AccessPoint);

            client.Timeout = TimeSpan.FromSeconds(15);
            
            client.DefaultRequestHeaders.Add("Accept",
                "application/vnd.github.v3+json");
            
            client.DefaultRequestHeaders.Add("User-Agent",
                "HttpClientFactory-Sample");

            Client = client;
        }
    }
}