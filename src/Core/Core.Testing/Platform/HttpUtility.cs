// <copyright file="HttpUtility.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Platform
{
    using System.Net.Http;

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
