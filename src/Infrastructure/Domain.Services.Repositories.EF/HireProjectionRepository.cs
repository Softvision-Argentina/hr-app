using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
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
            return Query();
        }
    }
}
