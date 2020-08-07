// <copyright file="UpdateReservationViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Reservation
{
    using System;
    using ApiServer.Contracts.Room;

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
