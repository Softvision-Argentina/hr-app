using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class CommunityRepository : Repository<Community, DataBaseContext>
    {
        public CommunityRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<Community> Query()
        {
            return base.Query();
        }

    }
}
