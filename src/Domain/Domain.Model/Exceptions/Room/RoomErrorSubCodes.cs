// <copyright file="RoomErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Room
{
    public enum RoomErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteRoomNotFound,
        RoomDeleted,
        InvalidUpdate,
        UpdateRoomNotFound,
        UpdateHasNotChanges,
        RoomNotFound,
    }
}
