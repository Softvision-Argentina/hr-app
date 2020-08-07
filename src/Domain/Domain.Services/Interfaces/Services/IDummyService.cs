// <copyright file="IDummyService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Domain.Services.Contracts.Seed;

    public interface IDummyService
    {
        CreatedDummyContract Create(CreateDummyContract contract);

        ReadedDummyContract Read(Guid id);

        void Update(UpdateDummyContract contract);

        void Delete(Guid id);

        IEnumerable<ReadedDummyContract> List();
    }
}
