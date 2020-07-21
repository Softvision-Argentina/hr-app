using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface ITechnicalStageRepository : IRepository<TechnicalStage>
    {
        void UpdateTechnicalStage(TechnicalStage newStage, TechnicalStage existingStage);
    }
}
