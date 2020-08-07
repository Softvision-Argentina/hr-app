// <copyright file="TechnicalStageRepository.cs" company="Softvision">
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

    public class TechnicalStageRepository : Repository<TechnicalStage, DataBaseContext>, ITechnicalStageRepository
    {
        public TechnicalStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<TechnicalStage> QueryEager()
        {
            return this.Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdateTechnicalStage(TechnicalStage newStage, TechnicalStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
