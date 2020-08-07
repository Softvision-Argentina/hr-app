// <copyright file="PreOfferStageRepository.cs" company="Softvision">
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

    public class PreOfferStageRepository : Repository<PreOfferStage, DataBaseContext>, IPreOfferStageRepository
    {
        public PreOfferStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<PreOfferStage> QueryEager()
        {
            return this.Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdatePreOfferStage(PreOfferStage newStage, PreOfferStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
