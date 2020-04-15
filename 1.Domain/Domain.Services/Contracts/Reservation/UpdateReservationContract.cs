using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.Room;

namespace Domain.Services.Contracts.Reservation
{
    public class UpdateReservationContract
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; }
        public DateTime UntilReservation { get; set; }
        //public TimeSpan Time { get; set; }
        public int User { get; set; }
        public int RoomId { get; set; }
        public UpdateRoomContract Room { get; set; }

    }
}
