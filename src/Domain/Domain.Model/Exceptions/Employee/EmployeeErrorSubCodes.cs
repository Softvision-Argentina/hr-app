// <copyright file="EmployeeErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Employee
{
    public enum EmployeeErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteEmployeeNotFound,
        EmployeeDeleted,
        InvalidUpdate,
        UpdateEmployeeNotFound,
        UpdateHasNotChanges,
        EmployeeNotFound,
    }
}
