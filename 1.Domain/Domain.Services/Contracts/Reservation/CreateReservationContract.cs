using Domain.Services.Contracts.Room;
using System;

namespace Domain.Services.Contracts.Reservation
{
    public class CreateReservationContract
    {
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; } 
        public DateTime UntilReservation { get; set; }
        public int User { get; set; }
        public int RoomId { get; set; }
        public CreateRoomContract Room { get; set; }
    }
}
