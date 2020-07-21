using Domain.Services.Contracts.Room;
using System;

namespace Domain.Services.Contracts.Reservation
{
    public class ReadedReservationContract
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; }
        public DateTime UntilReservation { get; set; }
        public int User { get; set; }
        public int RoomId { get; set; }
        public ReadedRoomContract Room { get; set; }
    }
}
