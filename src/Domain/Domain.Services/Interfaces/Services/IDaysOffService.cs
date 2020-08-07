// <copyright file="IDaysOffService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.DaysOff;

    public interface IDaysOffService
    {
        CreatedDaysOffContract Create(CreateDaysOffContract contract);

        ReadedDaysOffContract Read(int id);

        IEnumerable<ReadedDaysOffContract> ReadByDni(int dni);

        void Update(UpdateDaysOffContract contract);

        void AcceptPetition(int id);

        void Delete(int id);

        IEnumerable<ReadedDaysOffContract> List();
    }
}
