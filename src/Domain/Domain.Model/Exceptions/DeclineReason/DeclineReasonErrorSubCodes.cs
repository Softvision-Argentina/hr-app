// <copyright file="DeclineReasonErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions
{
    public enum DeclineReasonErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteDeclineReasonNotFound,
        DeclineReasonDeleted,
        InvalidUpdate,
        UpdateDeclineReasonNotFound,
        UpdateHasNotChanges,
        DeclineReasonNotFound,
    }
}
