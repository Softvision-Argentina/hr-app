using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IOfferStageRepository : IRepository<OfferStage>
    {
        void UpdateOfferStage(OfferStage newStage, OfferStage existingStage);
    }
}
