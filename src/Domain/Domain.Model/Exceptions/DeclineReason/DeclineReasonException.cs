// <copyright file="DeclineReasonException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions
{
    using System;
    using Core;

    public class DeclineReasonException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.DeclineReason;

        public DeclineReasonException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a skill type related error" : message)
        {
        }
    }

    public class InvalidDeclineReasonException : DeclineReasonException
    {
        public InvalidDeclineReasonException(string message)
            : base(string.IsNullOrEmpty(message) ? "The skill type is not valid" : message)
        {
        }
    }

    public class DeleteDeclineReasonNotFoundException : InvalidDeclineReasonException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.DeleteDeclineReasonNotFound;

        public DeleteDeclineReasonNotFoundException(int skillTypeId)
            : base($"Skill type not found for the skillTypeId: {skillTypeId}")
        {
            this.DeclineReasonId = skillTypeId;
        }

        public int DeclineReasonId { get; set; }
    }

    public class DeclineReasonDeletedException : InvalidDeclineReasonException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.DeclineReasonDeleted;

        public DeclineReasonDeletedException(int id, string name)
            : base($"The skill type {name} was deleted")
        {
            this.DeclineReasonId = id;
            this.Name = name;
        }

        public int DeclineReasonId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidDeclineReasonException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the skill.")
        {
        }
    }

    public class UpdateSkillNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.UpdateDeclineReasonNotFound;

        public UpdateSkillNotFoundException(int skillTypeId, Guid clientSystemId)
            : base($"skill {skillTypeId} and Client System Id {clientSystemId} was not found.")
        {
            this.DeclineReasonId = skillTypeId;
            this.ClientSystemId = clientSystemId;
        }

        public int DeclineReasonId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int skillId, Guid clientSystemId, string name)
            : base($"Skill type {name} has not changes.")
        {
            this.DeclineReasonId = skillId;
            this.ClientSystemId = clientSystemId;
        }

        public int DeclineReasonId { get; }

        public Guid ClientSystemId { get; }
    }

    public class DeclineReasonNotFoundException : InvalidDeclineReasonException
    {
        protected override int SubErrorCode => (int)DeclineReasonErrorSubCodes.DeclineReasonNotFound;

        public DeclineReasonNotFoundException(int skillTypeId) : base($"The skill type {skillTypeId} was not found.")
        {
            this.DeclineReasonId = skillTypeId;
        }

        public int DeclineReasonId { get; }
    }
}
