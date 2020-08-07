// <copyright file="OfficeException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Office
{
    using System;
    using Core;

    public class OfficeException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Office;

        public OfficeException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a Office related error" : message)
        {
        }
    }

    public class InvalidOfficeException : OfficeException
    {
        public InvalidOfficeException(string message)
            : base(string.IsNullOrEmpty(message) ? "The Office is not valid" : message)
        {
        }
    }

    public class DeleteOfficeNotFoundException : InvalidOfficeException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.DeleteOfficeNotFound;

        public DeleteOfficeNotFoundException(int officeId)
            : base($"Office not found for the Office Id: {officeId}")
        {
            this.OfficeId = officeId;
        }

        public int OfficeId { get; set; }
    }

    public class OfficeDeletedException : InvalidOfficeException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.OfficeDeleted;

        public OfficeDeletedException(int id, string name)
            : base($"The Office {name} was deleted")
        {
            this.OfficeId = id;
            this.Name = name;
        }

        public int OfficeId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidOfficeException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the Office.")
        {
        }
    }

    public class UpdateOfficeNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.UpdateOfficeNotFound;

        public UpdateOfficeNotFoundException(int officeId, Guid clientSystemId)
            : base($"Office {officeId} and Client System Id {clientSystemId} was not found.")
        {
            this.OfficeId = officeId;
            this.ClientSystemId = clientSystemId;
        }

        public int OfficeId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int officeId, Guid clientSystemId, string name)
            : base($"Office {name} has not changes.")
        {
            this.OfficeId = officeId;
            this.ClientSystemId = clientSystemId;
        }

        public int OfficeId { get; }

        public Guid ClientSystemId { get; }
    }

    public class OfficeNotFoundException : InvalidOfficeException
    {
        protected override int SubErrorCode => (int)OfficeErrorSubCodes.OfficeNotFound;

        public OfficeNotFoundException(int officeId) : base($"The Office {officeId} was not found.")
        {
            this.OfficeId = officeId;
        }

        public int OfficeId { get; }
    }
}
