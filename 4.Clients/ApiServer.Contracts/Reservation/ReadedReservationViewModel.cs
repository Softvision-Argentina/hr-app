using ApiServer.Contracts.User;
using ApiServer.Contracts.TaskItem;
using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.Room;

namespace ApiServer.Contracts.Reservation
{
    public class ReadedReservationViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; }
        public DateTime UntilReservation { get; set; }
        //public TimeSpan Time { get; set; }
        public int User { get; set; }
        public int RoomId { get; set; }
        public ReadedRoomViewModel Room { get; set; }
    }
}
