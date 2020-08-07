// <copyright file="ReadedRoomViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Room
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Office;
    using ApiServer.Contracts.Reservation;

    public class ReadedRoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int OfficeId { get; set; }

        public ReadedOfficeViewModel Office { get; set; }

        public ICollection<ReadedReservationViewModel> ReservationItems { get; set; }
    }
}
