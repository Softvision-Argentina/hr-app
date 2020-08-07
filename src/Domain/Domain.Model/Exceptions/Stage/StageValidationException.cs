// <copyright file="StageValidationException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Stage
{
    using System.Collections.Generic;
    using Core;

    public class CreateStageInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)StageValidationExceptionCodes.CreateContractInvalid;

        public CreateStageInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateStageInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)StageValidationExceptionCodes.UpdateContractInvalid;

        public UpdateStageInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
