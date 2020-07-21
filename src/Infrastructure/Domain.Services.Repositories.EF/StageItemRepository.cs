using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class StageItemRepository : Repository<StageItem, DataBaseContext>, IStageItemRepository
    {
        public StageItemRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<StageItem> Query()
        {
            return _dbContext.StageItems;
        }

        public void UpdateStageItem(StageItem newStageItem, StageItem existingStageItem)
        {
            _dbContext.Entry(existingStageItem).CurrentValues.SetValues(newStageItem);
        }
    }
}
