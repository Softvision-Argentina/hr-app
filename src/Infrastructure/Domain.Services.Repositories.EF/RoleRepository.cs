using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
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
            return Query();
        }
    }
}
