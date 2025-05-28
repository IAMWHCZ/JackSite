using System.Net.Http;

namespace JackSite.Authentication.PermissionServer.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient HttpClient { get; set; }

        static HttpClientHelper()
        {
            HttpClient = new HttpClient();
        }
    }
}