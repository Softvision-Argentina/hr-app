using Core;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.Skill
{
    public class CreateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)SkillValidationExceptionCodes.CreateContractInvalid;

        public CreateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)SkillValidationExceptionCodes.UpdateContractInvalid;

        public UpdateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
