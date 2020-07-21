using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class PreOfferRepository : Repository<PreOffer, DataBaseContext>
    {
        public PreOfferRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<PreOffer> Query()
        {
            return base.Query();
        }

        public override IQueryable<PreOffer> QueryEager()
        {
            return Query();
        }
    }
}
