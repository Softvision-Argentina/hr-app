// <copyright file="DummyException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Seed.Exceptions
{
    using System;
    using Core;

    public class DummyException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Dummy;

        public DummyException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a dummy related error" : message)
        {
        }
    }

    public class InvalidDummyException : DummyException
    {
        public InvalidDummyException(string message)
            : base(string.IsNullOrEmpty(message) ? "The dummy is not valid" : message)
        {
        }
    }

    public class DeleteDummyNotFoundException : InvalidDummyException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.DeleteDummyNotFound;

        public DeleteDummyNotFoundException(Guid dummyId)
            : base($"Dummy not found for the DummyId: {dummyId}")
        {
            this.DummyId = dummyId;
        }

        public Guid DummyId { get; set; }
    }

    public class DummyDeletedException : InvalidDummyException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.DummyDeleted;

        public DummyDeletedException(Guid id, string name)
            : base($"The dummy {name} was deleted")
        {
            this.DummyId = id;
            this.Name = name;
        }

        public Guid DummyId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidDummyException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the dummy.")
        {
        }
    }

    public class UpdateDummyNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.UpdateDummyNotFound;

        public UpdateDummyNotFoundException(Guid dummyId, Guid clientSystemId)
            : base($"Dummy {dummyId} and Client System Id {clientSystemId} was not found.")
        {
            this.DummyId = dummyId;
            this.ClientSystemId = clientSystemId;
        }

        public Guid DummyId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(Guid dummyId, Guid clientSystemId, string name)
            : base($"Dummy {name} has not changes.")
        {
            this.DummyId = dummyId;
            this.ClientSystemId = clientSystemId;
        }

        public Guid DummyId { get; }

        public Guid ClientSystemId { get; }
    }

    public class DummyNotFoundException : InvalidDummyException
    {
        protected override int SubErrorCode => (int)DummyErrorSubCodes.DummyNotFound;

        public DummyNotFoundException(Guid dummyId)
            : base($"The Dummy {dummyId} was not found.")
        {
            this.DummyId = dummyId;
        }

        public Guid DummyId { get; }
    }
}
