using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IHrStageRepository : IRepository<HrStage>
    {
        void UpdateHrStage(HrStage newStage, HrStage existingStage);
    }
}
