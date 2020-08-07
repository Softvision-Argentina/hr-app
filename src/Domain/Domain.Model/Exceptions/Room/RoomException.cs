// <copyright file="RoomException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Room
{
    using System;
    using Core;

    public class RoomException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Room;

        public RoomException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a room related error" : message)
        {
        }
    }

    public class InvalidRoomException : RoomException
    {
        public InvalidRoomException(string message)
            : base(string.IsNullOrEmpty(message) ? "The room is not valid" : message)
        {
        }
    }

    public class DeleteRoomNotFoundException : InvalidRoomException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.DeleteRoomNotFound;

        public DeleteRoomNotFoundException(int roomId)
            : base($"Room not found for the Room Id: {roomId}")
        {
            this.RoomId = roomId;
        }

        public int RoomId { get; set; }
    }

    public class RoomDeletedException : InvalidRoomException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.RoomDeleted;

        public RoomDeletedException(int id, string name)
            : base($"The room {name} was deleted")
        {
            this.RoomId = id;
            this.Name = name;
        }

        public int RoomId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidRoomException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the room.")
        {
        }
    }

    public class UpdateRoomNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.UpdateRoomNotFound;

        public UpdateRoomNotFoundException(int roomId, Guid clientSystemId)
            : base($"Room {roomId} and Client System Id {clientSystemId} was not found.")
        {
            this.RoomId = roomId;
            this.ClientSystemId = clientSystemId;
        }

        public int RoomId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int roomId, Guid clientSystemId, string name)
            : base($"Room {name} has not changes.")
        {
            this.RoomId = roomId;
            this.ClientSystemId = clientSystemId;
        }

        public int RoomId { get; }

        public Guid ClientSystemId { get; }
    }

    public class RoomNotFoundException : InvalidRoomException
    {
        protected override int SubErrorCode => (int)RoomErrorSubCodes.RoomNotFound;

        public RoomNotFoundException(int roomId) : base($"The Room {roomId} was not found.")
        {
            this.RoomId = roomId;
        }

        public int RoomId { get; }
    }
}
