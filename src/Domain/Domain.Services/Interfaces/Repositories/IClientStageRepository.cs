﻿// <copyright file="IClientStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IClientStageRepository : IRepository<ClientStage>
    {
        void UpdateClientStage(ClientStage newStage, ClientStage existingStage);
    }
}
