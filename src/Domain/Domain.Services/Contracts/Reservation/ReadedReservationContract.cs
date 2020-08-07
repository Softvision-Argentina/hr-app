// <copyright file="ReadedReservationContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Reservation
{
    using System;
    using Domain.Services.Contracts.Room;

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
