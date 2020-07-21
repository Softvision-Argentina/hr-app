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
        ReservationNotFound
    }
}
