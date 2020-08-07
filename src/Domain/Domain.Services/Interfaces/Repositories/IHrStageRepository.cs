// <copyright file="IHrStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IHrStageRepository : IRepository<HrStage>
    {
        void UpdateHrStage(HrStage newStage, HrStage existingStage);
    }
}
