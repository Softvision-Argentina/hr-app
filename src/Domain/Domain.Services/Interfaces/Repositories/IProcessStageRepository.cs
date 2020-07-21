using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IProcessStageRepository : IRepository<Stage>
    {
        void UpdateStage(Stage newStage, Stage existingStage);
    }
}
