// <copyright file="IPreOfferService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.PreOffer;

    public interface IPreOfferService
    {
        CreatedPreOfferContract Create(CreatePreOfferContract contract);

        ReadedPreOfferContract Read(int id);

        void Update(UpdatePreOfferContract contract);

        void Delete(int id);

        IEnumerable<ReadedPreOfferContract> List();

        IEnumerable<ReadedPreOfferContract> GetByProcessId(int id);
    }
}
