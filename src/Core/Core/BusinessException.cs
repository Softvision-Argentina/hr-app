// <copyright file="BusinessException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BusinessException : Exception
    {
        protected virtual int MainErrorCode => (int)ApplicationErrorMainCodes.Generic;

        protected virtual int SubErrorCode => 0;

        public int ErrorCode => this.MainErrorCode + this.SubErrorCode;

        public BusinessException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "There is an business related error" : message)
        {
        }

        public BusinessException(string message, Exception exception) : base(message, exception)
        {
        }
    }

    public class BusinessValidationException : BusinessException
    {
        private readonly List<KeyValuePair<string, string>> validationMessages;

        public IReadOnlyList<KeyValuePair<string, string>> ValidationMessages => this.validationMessages.AsReadOnly();

        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Validation;

        public BusinessValidationException(IEnumerable<KeyValuePair<string, string>> validationMessages)
            : base(string.Join("\r\n", validationMessages.Select(vm => vm.Value)))
        {
            this.validationMessages = validationMessages.ToList();
        }
    }
}
