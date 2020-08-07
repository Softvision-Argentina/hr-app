// <copyright file="IProcessStageService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Contracts.Stage.StageItem;

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
