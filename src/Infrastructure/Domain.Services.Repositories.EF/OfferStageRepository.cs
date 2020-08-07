// <copyright file="OfferStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class OfferStageRepository : Repository<OfferStage, DataBaseContext>, IOfferStageRepository
    {
        public OfferStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<OfferStage> QueryEager()
        {
            return this.Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdateOfferStage(OfferStage newStage, OfferStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
