// <copyright file="PreOfferValidationException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.PreOffer
{
    using System.Collections.Generic;
    using Core;
    using Domain.Model.Exceptions.Skill;

    public class CreateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)PreOfferValidationExceptionCodes.CreateContractInvalid;

        public CreateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)PreOfferValidationExceptionCodes.UpdateContractInvalid;

        public UpdateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
