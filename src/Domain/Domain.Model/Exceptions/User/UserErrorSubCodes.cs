// <copyright file="UserErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.User
{
    public enum UserErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteUserNotFound,
        UserDeleted,
        InvalidUpdate,
        UpdateUserNotFound,
        UpdateHasNotChanges,
        UserNotFound,
    }
}
