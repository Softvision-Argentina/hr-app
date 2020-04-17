using Core;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.Task
{
    public class CreateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)TaskValidationExceptionCodes.CreateContractInvalid;

        public CreateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)TaskValidationExceptionCodes.UpdateContractInvalid;

        public UpdateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
