// <copyright file="ProcessValidationException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Process
{
    using System.Collections.Generic;
    using Core;

    public class CreateProcessInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ProcessValidationExceptionCodes.CreateContractInvalid;

        public CreateProcessInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateProcessInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ProcessValidationExceptionCodes.UpdateContractInvalid;

        public UpdateProcessInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
