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
        public HttpResponseMessage Response { get; set; }
        public string ResponseString { get; set; }
    }
}
