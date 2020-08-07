// <copyright file="IOpenPositionService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.OpenPositions;

    public interface IOpenPositionService
    {
        CreatedOpenPositionContract Create(CreateOpenPositionContract openPosition);

        ReadedOpenPositionContract GetById(int id);

        IEnumerable<ReadedOpenPositionContract> Get();

        void Update(UpdateOpenPositionContract openPosition);

        void Delete(int id);
    }
}
