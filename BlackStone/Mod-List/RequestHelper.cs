using System.Net.Http;

// so i can send requests easier
namespace BlackStone
{
    internal class RequestHelper
    {
        public static HttpClient Httpclient = new();
        public static string sendrequest(string url)
        {
            return Httpclient.GetAsync(url)
                                 .GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
        }
    }
}
