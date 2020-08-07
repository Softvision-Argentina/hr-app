// <copyright file="IStageItemRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IStageItemRepository : IRepository<StageItem>
    {
        void UpdateStageItem(StageItem newStageItem, StageItem existingStageItem);
    }
}
