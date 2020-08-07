// <copyright file="ReaddressReasonException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.ReaddressReason
{
    using System.Collections.Generic;
    using Core;

    public class ReaddressReasonException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.ReaddressReason;

        public ReaddressReasonException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a re-address reason related error" : message)
        {
        }
    }

    public class CreateInvalidReaddressReasonException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReaddressReasonExceptionCodes.CreateContractInvalid;

        public CreateInvalidReaddressReasonException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateInvalidReaddressReasonException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReaddressReasonExceptionCodes.UpdateContractInvalid;

        public UpdateInvalidReaddressReasonException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
