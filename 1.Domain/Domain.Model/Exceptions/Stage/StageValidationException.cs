using Core;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.Stage
{
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
