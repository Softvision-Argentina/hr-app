// <copyright file="HireProjectionRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class HireProjectionRepository : Repository<HireProjection, DataBaseContext>
    {
        public HireProjectionRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<HireProjection> Query()
        {
            return base.Query();
        }

        public override IQueryable<HireProjection> QueryEager()
        {
            return this.Query();
        }
    }
}
