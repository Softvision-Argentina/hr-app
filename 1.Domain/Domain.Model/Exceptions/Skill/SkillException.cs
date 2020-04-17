using Core;
using System;

namespace Domain.Model.Exceptions.Skill
{
    public class SkillException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Skill;

        public SkillException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a skill related error" : message)
        {
        }
    }

    public class InvalidSkillException : SkillException
    {
        public InvalidSkillException(string message)
            : base(string.IsNullOrEmpty(message) ? "The skill is not valid" : message)
        {
        }
    }

    public class DeleteSkillNotFoundException : InvalidSkillException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.DeleteSkillNotFound;

        public DeleteSkillNotFoundException(int skillId)
            : base($"Skill not found for the skillId: {skillId}")
        {
            SkillId = skillId;
        }

        public int SkillId { get; set; }
    }

    public class SkillDeletedException : InvalidSkillException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.SkillDeleted;

        public SkillDeletedException(int id, string name)
            : base($"The skill {name} was deleted")
        {
            SkillId = id;
            Name = name;
        }

        public int SkillId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidSkillException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the skill.")
        {
        }
    }

    public class UpdateSkillNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.UpdateSkillNotFound;

        public UpdateSkillNotFoundException(int skillId, Guid clientSystemId)
            : base($"skill {skillId} and Client System Id {clientSystemId} was not found.")
        {
            SkillId = skillId;
            ClientSystemId = clientSystemId;
        }
  
        public int SkillId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int skillId, Guid clientSystemId, string name)
            : base($"Skill {name} has not changes.")
        {
            SkillId = skillId;
            ClientSystemId = clientSystemId;
        }

        public int SkillId { get; }
        public Guid ClientSystemId { get; }
    }

    public class SkillNotFoundException : InvalidSkillException
    {
        protected override int SubErrorCode => (int)SkillErrorSubCodes.SkillNotFound;

        public SkillNotFoundException(int skillId) : base($"The skill {skillId} was not found.")
        {
            SkillId = skillId;
        }

        public int SkillId { get; }
    }
}

