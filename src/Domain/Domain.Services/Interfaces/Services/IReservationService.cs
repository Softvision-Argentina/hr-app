// <copyright file="IReservationService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Reservation;

    public interface IReservationService
    {
        CreatedReservationContract Create(CreateReservationContract contract);

        ReadedReservationContract Read(int id);

        void Update(UpdateReservationContract contract);

        void Delete(int id);

        IEnumerable<ReadedReservationContract> List();
    }
}
