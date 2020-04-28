using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

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

        public override Dashboard Update(Dashboard entity)
        {
            var previousDashboards = _dbContext.UserDashboards.Where(ud => ud.DashboardId == entity.Id);

            _dbContext.UserDashboards.RemoveRange(previousDashboards);

            foreach (var item in entity.UserDashboards)
            {
                item.User = null;
                _dbContext.UserDashboards.Add(item);
            }

            return base.Update(entity);
        }
    }
}
