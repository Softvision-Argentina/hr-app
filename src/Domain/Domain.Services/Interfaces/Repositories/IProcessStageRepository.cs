// <copyright file="IProcessStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IProcessStageRepository : IRepository<Stage>
    {
        void UpdateStage(Stage newStage, Stage existingStage);
    }
}
