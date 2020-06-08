using Core;
using Domain.Model.Exceptions.Skill;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.PreOffer
{
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
