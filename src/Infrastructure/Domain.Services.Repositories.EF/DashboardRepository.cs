// <copyright file="DashboardRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class DashboardRepository : Repository<Dashboard, DataBaseContext>
    {
        public DashboardRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Dashboard> Query()
        {
            return base.Query();
        }

        public override IQueryable<Dashboard> QueryEager()
        {
            return this.Query().Include(d => d.UserDashboards).ThenInclude(u => u.User);
        }

        public override Dashboard Update(Dashboard entity)
        {
            var previousDashboards = this.DbContext.UserDashboards.Where(ud => ud.DashboardId == entity.Id);

            this.DbContext.UserDashboards.RemoveRange(previousDashboards);

            foreach (var item in entity.UserDashboards)
            {
                item.User = null;
                this.DbContext.UserDashboards.Add(item);
            }

            return base.Update(entity);
        }
    }
}
