using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class OfferRepository : Repository<Offer, DataBaseContext>
    {
        public OfferRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Offer> Query()
        {
            return base.Query();
        }

        public override IQueryable<Offer> QueryEager()
        {
            return Query();
        }
    }
}
