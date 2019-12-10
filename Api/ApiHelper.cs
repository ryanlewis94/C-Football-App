using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FootballApp.Api
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }
        
        public static void InitializeClient()
        {
            ApiClient = new HttpClient(
                new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                });
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            ApiClient.Timeout = new TimeSpan(0, 0, 30);
        }
    }
}
