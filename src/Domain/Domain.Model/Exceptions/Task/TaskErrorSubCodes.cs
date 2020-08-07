// <copyright file="TaskErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Task
{
    public enum TaskErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteTaskNotFound,
        TaskDeleted,
        InvalidUpdate,
        UpdateTaskNotFound,
        UpdateHasNotChanges,
        TaskNotFound,
    }
}
