// <copyright file="DaysOffErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.DaysOff
{
    public enum DaysOffErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteDaysOffNotFound,
        DaysOffDeleted,
        InvalidUpdate,
        UpdateDaysOffNotFound,
        UpdateHasNotChanges,
        DaysOffNotFound,
    }
}
