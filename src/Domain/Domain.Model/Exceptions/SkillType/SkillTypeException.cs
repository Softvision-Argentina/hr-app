// <copyright file="SkillTypeException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.SkillType
{
    using System;
    using Core;

    public class SkillTypeException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.SkillType;

        public SkillTypeException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a skill type related error" : message)
        {
        }
    }

    public class InvalidSkillTypeException : SkillTypeException
    {
        public InvalidSkillTypeException(string message)
            : base(string.IsNullOrEmpty(message) ? "The skill type is not valid" : message)
        {
        }
    }

    public class DeleteSkillTypeNotFoundException : InvalidSkillTypeException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.DeleteSkillTypeNotFound;

        public DeleteSkillTypeNotFoundException(int skillTypeId)
            : base($"Skill type not found for the skillTypeId: {skillTypeId}")
        {
            this.SkillTypeId = skillTypeId;
        }

        public int SkillTypeId { get; set; }
    }

    public class SkillTypeDeletedException : InvalidSkillTypeException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.SkillTypeDeleted;

        public SkillTypeDeletedException(int id, string name)
            : base($"The skill type {name} was deleted")
        {
            this.SkillTypeId = id;
            this.Name = name;
        }

        public int SkillTypeId { get; set; }

        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidSkillTypeException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the skill.")
        {
        }
    }

    public class UpdateSkillNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.UpdateSkillTypeNotFound;

        public UpdateSkillNotFoundException(int skillTypeId, Guid clientSystemId)
            : base($"skill {skillTypeId} and Client System Id {clientSystemId} was not found.")
        {
            this.SkillTypeId = skillTypeId;
            this.ClientSystemId = clientSystemId;
        }

        public int SkillTypeId { get; }

        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int skillId, Guid clientSystemId, string name)
            : base($"Skill type {name} has not changes.")
        {
            this.SkillTypeId = skillId;
            this.ClientSystemId = clientSystemId;
        }

        public int SkillTypeId { get; }

        public Guid ClientSystemId { get; }
    }

    public class SkillTypeNotFoundException : InvalidSkillTypeException
    {
        protected override int SubErrorCode => (int)SkillTypeErrorSubCodes.SkillTypeNotFound;

        public SkillTypeNotFoundException(int skillTypeId) : base($"The skill type {skillTypeId} was not found.")
        {
            this.SkillTypeId = skillTypeId;
        }

        public int SkillTypeId { get; }
    }
}
