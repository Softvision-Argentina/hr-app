using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IClientStageRepository : IRepository<ClientStage>
    {
        void UpdateClientStage(ClientStage newStage, ClientStage existingStage);
    }
}
