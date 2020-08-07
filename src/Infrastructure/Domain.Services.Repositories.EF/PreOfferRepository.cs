// <copyright file="PreOfferRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class PreOfferRepository : Repository<PreOffer, DataBaseContext>
    {
        public PreOfferRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<PreOffer> Query()
        {
            return base.Query();
        }

        public override IQueryable<PreOffer> QueryEager()
        {
            return this.Query();
        }
    }
}
