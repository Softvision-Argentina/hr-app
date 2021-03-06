﻿// <copyright file="UpdateReservationContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Reservation
{
    using System;
    using Domain.Services.Contracts.Room;

    public class UpdateReservationContract
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime SinceReservation { get; set; }

        public DateTime UntilReservation { get; set; }

        public int User { get; set; }

        public int RoomId { get; set; }

        public UpdateRoomContract Room { get; set; }
    }
}
