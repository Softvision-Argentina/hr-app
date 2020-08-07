﻿// <copyright file="UpdateStageItemContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage.StageItem
{
    public class UpdateStageItemContract
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string AssociatedContent { get; set; }

        public int StageId { get; set; }
    }
}
