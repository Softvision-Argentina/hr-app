// <copyright file="Room.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class Room : DescriptiveEntity<int>
    {
        public int OfficeId { get; set; }

        public Office Office { get; set; }

        public IList<Reservation> ReservationItems { get; set; }
    }
}
