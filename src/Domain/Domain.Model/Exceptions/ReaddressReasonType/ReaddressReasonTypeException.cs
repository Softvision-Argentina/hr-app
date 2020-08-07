// <copyright file="ReaddressReasonTypeException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.ReaddressReasonType
{
    using System.Collections.Generic;
    using Core;

    public class ReaddressReasonTypeException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.ReaddressReasonType;

        public ReaddressReasonTypeException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a re-address reason related error" : message)
        {
        }
    }

    public class CreateInvalidReaddressReasonTypeException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReaddressReasonTypeExceptionCode.CreateContractInvalid;

        public CreateInvalidReaddressReasonTypeException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateInvalidReaddressReasonTypeException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReaddressReasonTypeExceptionCode.UpdateContractInvalid;

        public UpdateInvalidReaddressReasonTypeException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
