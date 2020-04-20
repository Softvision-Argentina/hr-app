﻿using Core;
using System;

namespace Domain.Model
{
    public class Reservation : Entity<int>
    {
        public string Description { get; set; }
        public DateTime SinceReservation { get; set; }
        public DateTime UntilReservation { get; set; }
        public User User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
