using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IStageItemRepository : IRepository<StageItem>
    {
        void UpdateStageItem(StageItem newStageItem, StageItem existingStageItem);
    }
}
