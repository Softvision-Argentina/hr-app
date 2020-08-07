// <copyright file="IOfferStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IOfferStageRepository : IRepository<OfferStage>
    {
        void UpdateOfferStage(OfferStage newStage, OfferStage existingStage);
    }
}
