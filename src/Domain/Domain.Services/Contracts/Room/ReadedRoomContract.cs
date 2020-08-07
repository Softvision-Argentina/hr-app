// <copyright file="ReadedRoomContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Room
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Office;
    using Domain.Services.Contracts.Reservation;

    public class ReadedRoomContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int OfficeId { get; set; }

        public ReadedOfficeContract Office { get; set; }

        public ICollection<ReadedReservationContract> ReservationItems { get; set; }
    }
}
