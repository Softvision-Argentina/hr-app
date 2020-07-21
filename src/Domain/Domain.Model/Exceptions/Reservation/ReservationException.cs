using Core;
using System;

namespace Domain.Model.Exceptions.Reservation
{
    public class ReservationException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Reservation;

        public ReservationException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a Reservation related error" : message)
        {
        }
    }

    public class InvalidReservationException : ReservationException
    {
        public InvalidReservationException(string message)
            : base(string.IsNullOrEmpty(message) ? "The Reservation is not valid" : message)
        {
        }
    }

    public class DeleteReservationNotFoundException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.DeleteReservationNotFound;

        public DeleteReservationNotFoundException(int reservationId)
            : base($"Reservation not found for the ReservationId: {reservationId}")
        {
            ReservationId = reservationId;
        }

        public int ReservationId { get; set; }
    }

    public class ReservationDeletedException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.ReservationDeleted;

        public ReservationDeletedException(int id, string name)
            : base($"The Reservation {name} was deleted")
        {
            ReservationId = id;
            Name = name;
        }

        public int ReservationId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the Reservation.")
        {
        }
    }

    public class UpdateReservationNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.UpdateReservationNotFound;

        public UpdateReservationNotFoundException(int reservationId, Guid clientSystemId)
            : base($"Reservation {reservationId} and Client System Id {clientSystemId} was not found.")
        {
            ReservationId = reservationId;
            ClientSystemId = clientSystemId;
        }

        public int ReservationId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int reservationId, Guid clientSystemId, string name)
            : base($"Reservation {name} has not changes.")
        {
            ReservationId = reservationId;
            ClientSystemId = clientSystemId;
        }

        public int ReservationId { get; }
        public Guid ClientSystemId { get; }
    }

    public class ReservationNotFoundException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.ReservationNotFound;

        public ReservationNotFoundException(int reservationId) : base($"The Reservation {reservationId} was not found.")
        {
            ReservationId = reservationId;
        }

        public int ReservationId { get; }
    }
}
