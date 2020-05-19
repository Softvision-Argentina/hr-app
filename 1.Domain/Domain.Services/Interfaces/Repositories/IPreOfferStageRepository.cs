using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IPreOfferStageRepository : IRepository<PreOfferStage>
    {
        void UpdatePreOfferStage(PreOfferStage newStage, PreOfferStage existingStage);
    }
}
