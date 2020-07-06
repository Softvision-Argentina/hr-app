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

    public class HttpResultData<T> where T : class
    {
        public HttpResponseMessage Response { get; set; }
        public string ResponseString { get; set; }
        public T ResponseEntity { get; set; }
        public ExceptionData ResponseError { get; set; }
    }
}
