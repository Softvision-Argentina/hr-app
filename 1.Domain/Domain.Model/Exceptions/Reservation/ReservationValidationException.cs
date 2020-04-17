using Core;
using System.Collections.Generic;

namespace Domain.Model.Exceptions.Reservation
{
    public class CreateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReservationValidationExceptionCodes.CreateContractInvalid;

        public CreateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)ReservationValidationExceptionCodes.UpdateContractInvalid;

        public UpdateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
