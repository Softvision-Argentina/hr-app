
using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class OpenPositionRepository : Repository<OpenPosition, DataBaseContext>
    {
        public OpenPositionRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<OpenPosition> Query()
        {
            return base.Query();
        }

        public override IQueryable<OpenPosition> QueryEager()
        {
            return Query()
                .Include(r => r.Community);
        }
    }
}
