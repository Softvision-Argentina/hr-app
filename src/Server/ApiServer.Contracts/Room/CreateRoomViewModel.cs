// <copyright file="CreateRoomViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Room
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Office;
    using ApiServer.Contracts.Reservation;

    public class CreateRoomViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int OfficeId { get; set; }

        public CreateOfficeViewModel Office { get; set; }

        public ICollection<CreateReservationViewModel> ReservationItems { get; set; }
    }
}
