using Core;
using System;

namespace Domain.Model.Exceptions.Offer
{
    public class OfferException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Offer;

        public OfferException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a offer related error" : message)
        {
        }
    }

    public class InvalidOfferException : OfferException
    {
        public InvalidOfferException(string message)
            : base(string.IsNullOrEmpty(message) ? "The offer is not valid" : message)
        {
        }
    }

    public class DeleteOfferNotFoundException : InvalidOfferException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.DeleteOfferNotFound;

        public DeleteOfferNotFoundException(int offerId)
            : base($"Offer not found for the offerId: {offerId}")
        {
            OfferId = offerId;
        }

        public int OfferId { get; set; }
    }

    public class OfferDeletedException : InvalidOfferException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.OfferDeleted;

        public OfferDeletedException(int id, string name)
            : base($"The offer {name} was deleted")
        {
            OfferId = id;
            Name = name;
        }

        public int OfferId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidOfferException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the offer.")
        {
        }
    }

    public class UpdateOfferNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.UpdateOfferNotFound;

        public UpdateOfferNotFoundException(int offerId, Guid clientSystemId)
            : base($"offer {offerId} and Client System Id {clientSystemId} was not found.")
        {
            OfferId = offerId;
            ClientSystemId = clientSystemId;
        }

        public int OfferId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int offerId, Guid clientSystemId, string name)
            : base($"Offer {name} has not changes.")
        {
            OfferId = offerId;
            ClientSystemId = clientSystemId;
        }

        public int OfferId { get; }
        public Guid ClientSystemId { get; }
    }

    public class OfferNotFoundException : InvalidOfferException
    {
        protected override int SubErrorCode => (int)OfferErrorSubCodes.OfferNotFound;

        public OfferNotFoundException(int offerId) : base($"The offer {offerId} was not found.")
        {
            OfferId = offerId;
        }

        public int OfferId { get; }
    }
}
