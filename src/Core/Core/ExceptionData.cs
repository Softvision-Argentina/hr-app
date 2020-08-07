// <copyright file="ExceptionData.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core
{
    using System.Collections;
    using System.Collections.Generic;

    public class ValidationError
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class ExceptionData
    {
        public int HttpStatusCode { get; set; }

        public int ErrorCode { get; set; }

        public string ExceptionMessage { get; set; }

        public string InnerExceptionMessage { get; set; }

        public IDictionary AdditionalInfo { get; set; }

        public List<ValidationError> ValidationErrors { get; set; } = null;
    }
}
