using Core;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.Process
{
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
