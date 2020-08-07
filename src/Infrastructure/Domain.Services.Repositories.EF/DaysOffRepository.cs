// <copyright file="DaysOffRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class DaysOffRepository : Repository<DaysOff, DataBaseContext>
    {
        public DaysOffRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<DaysOff> Query()
        {
            return base.Query();
        }

        public override IQueryable<DaysOff> QueryEager()
        {
            return this.Query().Include(_ => _.Employee);
        }
    }
}
