using Core;
using System;

namespace Domain.Model.Exceptions.PreOffer
{
    public class PreOfferException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.PreOffer;

        public PreOfferException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a pre-offer related error" : message)
        {
        }
    }

    public class InvalidPreOfferException : PreOfferException
    {
        public InvalidPreOfferException(string message)
            : base(string.IsNullOrEmpty(message) ? "The pre-offer is not valid" : message)
        {
        }
    }

    public class DeletePreOfferNotFoundException : InvalidPreOfferException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.DeletePreOfferNotFound;

        public DeletePreOfferNotFoundException(int preOfferId)
            : base($"PreOffer not found for the preOfferId: {preOfferId}")
        {
            PreOfferId = preOfferId;
        }

        public int PreOfferId { get; set; }
    }

    public class PreOfferDeletedException : InvalidPreOfferException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.PreOfferDeleted;

        public PreOfferDeletedException(int id, string name)
            : base($"The pre-offer {name} was deleted")
        {
            PreOfferId = id;
            Name = name;
        }

        public int PreOfferId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidPreOfferException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the pre-offer.")
        {
        }
    }

    public class UpdatePreOfferNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.UpdatePreOfferNotFound;

        public UpdatePreOfferNotFoundException(int preOfferId, Guid clientSystemId)
            : base($"pre-offer {preOfferId} and Client System Id {clientSystemId} was not found.")
        {
            PreOfferId = preOfferId;
            ClientSystemId = clientSystemId;
        }

        public int PreOfferId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int preOfferId, Guid clientSystemId, string name)
            : base($"Pre-offer {name} has no changes.")
        {
            PreOfferId = preOfferId;
            ClientSystemId = clientSystemId;
        }

        public int PreOfferId { get; }
        public Guid ClientSystemId { get; }
    }

    public class PreOfferNotFoundException : InvalidPreOfferException
    {
        protected override int SubErrorCode => (int)PreOfferErrorSubCodes.PreOfferNotFound;

        public PreOfferNotFoundException(int preOfferId) : base($"The pre-offer {preOfferId} was not found.")
        {
            PreOfferId = preOfferId;
        }

        public int PreOfferId { get; }
    }
}
