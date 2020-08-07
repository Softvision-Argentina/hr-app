// <copyright file="UpdateRoomViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Room
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Office;
    using ApiServer.Contracts.Reservation;

    public class UpdateRoomViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int OfficeId { get; set; }

        public UpdateOfficeViewModel Office { get; set; }

        public ICollection<CreateReservationViewModel> ReservationItems { get; set; }
    }
}
