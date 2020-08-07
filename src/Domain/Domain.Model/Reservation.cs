// <copyright file="Reservation.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;

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
