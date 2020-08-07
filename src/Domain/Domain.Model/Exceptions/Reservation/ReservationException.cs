// <copyright file="ReservationException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Reservation
{
    using System;
    using Core;

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
            this.ReservationId = reservationId;
        }

        public int ReservationId { get; set; }
    }

    public class ReservationDeletedException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.ReservationDeleted;

        public ReservationDeletedException(int id, string name)
            : base($"The Reservation {name} was deleted")
        {
            this.ReservationId = id;
            this.Name = name;
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
            this.ReservationId = reservationId;
            this.ClientSystemId = clientSystemId;
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
            this.ReservationId = reservationId;
            this.ClientSystemId = clientSystemId;
        }

        public int ReservationId { get; }

        public Guid ClientSystemId { get; }
    }

    public class ReservationNotFoundException : InvalidReservationException
    {
        protected override int SubErrorCode => (int)ReservationErrorSubCodes.ReservationNotFound;

        public ReservationNotFoundException(int reservationId) : base($"The Reservation {reservationId} was not found.")
        {
            this.ReservationId = reservationId;
        }

        public int ReservationId { get; }
    }
}
