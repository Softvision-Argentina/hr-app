using System;
using System.Collections.Generic;
using System.Text;

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
        UserNotFound
    }
}
