// <copyright file="CreateReservationViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Reservation
{
    using System;
    using ApiServer.Contracts.Room;

    public class CreateReservationViewModel
    {
        public string Description { get; set; }

        public DateTime SinceReservation { get; set; }

        public DateTime UntilReservation { get; set; }

        public int User { get; set; }

        public int RoomId { get; set; }

        public CreateRoomViewModel Room { get; set; }
    }
}
