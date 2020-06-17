using System.Collections;
using System.Net.Http;

namespace Core.Testing.Platform
{
    public class HttpVerb
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string DELETE = "DELETE";
        public const string PUT = "PUT";
    }

    public class ResponseError
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public IDictionary AdditionalData { get; set; }
    }

    public class HttpResultData<T> where T : class
    {
        public HttpResponseMessage Response { get; set; }
        public string ResponseString { get; set; }
        public T ResponseEntity { get; set; }
        public ResponseError ResponseError { get; set; }
    }
}
