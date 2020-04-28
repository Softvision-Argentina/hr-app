using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.StageItem;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IProcessStageService
    {
        CreatedStageContract Create(CreateStageContract contract);
        ReadedStageContract Read(int id);
        void Update(UpdateStageContract contract);
        void Delete(int id);
        IEnumerable<ReadedStageContract> List();
        CreatedStageItemContract AddItemToStage(CreateStageItemContract createStageItemContract);
        void RemoveItemToStage(int stageItemId);
        void UpdateStageItem(UpdateStageItemContract updateStageItemContract);
    }
}
