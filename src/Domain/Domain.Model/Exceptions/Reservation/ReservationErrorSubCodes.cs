// <copyright file="ReservationErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Reservation
{
    public enum ReservationErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteReservationNotFound,
        ReservationDeleted,
        InvalidUpdate,
        UpdateReservationNotFound,
        UpdateHasNotChanges,
        ReservationNotFound,
    }
}
