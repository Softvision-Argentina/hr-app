// <copyright file="CommunityException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Community
{
    using System;
    using Core;

    public class CommunityException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Community;

        public CommunityException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a Community related error" : message)
        {
        }
    }

    public class InvalidCommunityException : CommunityException
    {
        public InvalidCommunityException(string message)
            : base(string.IsNullOrEmpty(message) ? "The Community is not valid" : message)
        {
        }
    }

    public class DeleteCommunityNotFoundException : InvalidCommunityException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.DeleteCommunityNotFound;

        public DeleteCommunityNotFoundException(int communityId)
            : base($"Community not found for the CommunityId: {communityId}")
        {
            this.CommunityId = communityId;
        }

        public int CommunityId { get; set; }
    }

    public class CommunityDeletedException : InvalidCommunityException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.CommunityDeleted;

        public CommunityDeletedException(int id, string name)
            : base($"The Community {name} was deleted")
        {
            this.CommunityId = id;
            this.Name = name;
        }

        public int CommunityId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidCommunityException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the Community.")
        {
        }
    }

    public class UpdateCommunityNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.UpdateCommunityNotFound;

        public UpdateCommunityNotFoundException(int communityId, Guid clientSystemId)
            : base($"Community {communityId} and Client System Id {clientSystemId} was not found.")
        {
            this.CommunityId = communityId;
            this.ClientSystemId = clientSystemId;
        }

        public int CommunityId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int communityId, Guid clientSystemId, string name)
            : base($"Community {name} has not changes.")
        {
            this.CommunityId = communityId;
            this.ClientSystemId = clientSystemId;
        }

        public int CommunityId { get; }

        public Guid ClientSystemId { get; }
    }

    public class CommunityNotFoundException : InvalidCommunityException
    {
        protected override int SubErrorCode => (int)CommunityErrorSubCodes.CommunityNotFound;

        public CommunityNotFoundException(int communityId) : base($"The Community {communityId} was not found.")
        {
            this.CommunityId = communityId;
        }

        public int CommunityId { get; }
    }
}
