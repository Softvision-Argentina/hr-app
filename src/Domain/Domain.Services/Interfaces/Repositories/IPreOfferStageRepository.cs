// <copyright file="IPreOfferStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IPreOfferStageRepository : IRepository<PreOfferStage>
    {
        void UpdatePreOfferStage(PreOfferStage newStage, PreOfferStage existingStage);
    }
}
