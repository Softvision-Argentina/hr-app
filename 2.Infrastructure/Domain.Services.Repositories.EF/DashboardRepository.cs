using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;

namespace Domain.Services.Repositories.EF
{
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
            return Query().Include(d => d.UserDashboards).ThenInclude(u => u.User);
        }
    }
}
