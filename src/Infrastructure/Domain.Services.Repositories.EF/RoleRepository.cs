// <copyright file="RoleRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class RoleRepository : Repository<Role, DataBaseContext>
    {
        public RoleRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Role> Query()
        {
            return base.Query();
        }

        public override IQueryable<Role> QueryEager()
        {
            return this.Query();
        }
    }
}
