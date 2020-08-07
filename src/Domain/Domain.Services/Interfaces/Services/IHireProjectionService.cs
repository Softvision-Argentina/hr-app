// <copyright file="IHireProjectionService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.HireProjection;

    public interface IHireProjectionService
    {
        CreatedHireProjectionContract Create(CreateHireProjectionContract contract);

        ReadedHireProjectionContract Read(int id);

        void Update(UpdateHireProjectionContract contract);

        void Delete(int id);

        IEnumerable<ReadedHireProjectionContract> List();
    }
}
