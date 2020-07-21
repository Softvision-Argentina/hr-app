using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class DaysOffRepository : Repository<DaysOff, DataBaseContext>
    {
        public DaysOffRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<DaysOff> Query()
        {
            return base.Query();
        }

        public override IQueryable<DaysOff> QueryEager()
        {
            return Query().Include(_ => _.Employee);

        }
    }
}
