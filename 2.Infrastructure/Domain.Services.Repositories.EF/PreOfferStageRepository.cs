using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class PreOfferStageRepository : Repository<PreOfferStage, DataBaseContext>, IPreOfferStageRepository
    {
        public PreOfferStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<PreOfferStage> QueryEager()
        {
            return Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdatePreOfferStage(PreOfferStage newStage, PreOfferStage existingStage)
        {
            _dbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
