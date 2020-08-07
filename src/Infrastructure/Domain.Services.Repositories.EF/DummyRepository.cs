// <copyright file="DummyRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model.Seed;
    using Persistance.EF;

    public class DummyRepository : Repository<Dummy, DataBaseContext>
    {
        public DummyRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Dummy> Query()
        {
            return base.Query();
        }

        public override IQueryable<Dummy> QueryEager()
        {
            return this.Query();
        }
    }
}
