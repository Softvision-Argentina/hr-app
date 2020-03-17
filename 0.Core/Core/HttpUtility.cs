using System.Net.Http;

namespace Core
{
    public class HttpVerb
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string DELETE = "DELETE";
        public const string PUT = "PUT";
    }

    public class HttpResultData
    {
        public HttpResponseMessage response { get; set; }
        public string responseString { get; set; }
    }
}
