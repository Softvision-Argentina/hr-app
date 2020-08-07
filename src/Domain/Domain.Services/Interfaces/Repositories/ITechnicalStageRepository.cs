// <copyright file="ITechnicalStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface ITechnicalStageRepository : IRepository<TechnicalStage>
    {
        void UpdateTechnicalStage(TechnicalStage newStage, TechnicalStage existingStage);
    }
}
