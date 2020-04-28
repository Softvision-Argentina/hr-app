using Core.Persistance;
using Domain.Model.Seed;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
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
            return Query();
        }
    }
}
