// <copyright file="StageErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Stage
{
    public enum StageErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteStageNotFound,
        StageDeleted,
        InvalidUpdate,
        UpdateStageNotFound,
        UpdateHasNotChanges,
        StageNotFound,
    }
}
