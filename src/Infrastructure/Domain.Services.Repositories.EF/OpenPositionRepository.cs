// <copyright file="OpenPositionRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class OpenPositionRepository : Repository<OpenPosition, DataBaseContext>
    {
        public OpenPositionRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<OpenPosition> Query()
        {
            return base.Query();
        }

        public override IQueryable<OpenPosition> QueryEager()
        {
            return this.Query()
                .Include(r => r.Community);
        }
    }
}
