using ApiServer.Contracts.Room;
using System;

namespace ApiServer.Contracts.Reservation
{
    public class UpdateReservationViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; }
        public DateTime UntilReservation { get; set; }
        public int User { get; set; }
        public int RoomId { get; set; }
        public UpdateRoomViewModel Room { get; set; }
    }
}
