// <copyright file="HrStageRepository.cs" company="Softvision">
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

    public class HrStageRepository : Repository<HrStage, DataBaseContext>, IHrStageRepository
    {
        public HrStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<HrStage> QueryEager()
        {
            return this.Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdateHrStage(HrStage newStage, HrStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
