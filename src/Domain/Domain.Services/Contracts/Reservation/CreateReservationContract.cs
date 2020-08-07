// <copyright file="CreateReservationContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Reservation
{
    using System;
    using Domain.Services.Contracts.Room;

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
