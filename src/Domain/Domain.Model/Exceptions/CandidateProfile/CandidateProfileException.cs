using Core;
using System;

namespace Domain.Model.Exceptions.CandidateProfile
{
    public class CandidateProfileException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.CandidateProfile;

        public CandidateProfileException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a skill related error" : message)
        {
        }
    }

    public class InvalidCandidateProfileException : CandidateProfileException
    {
        public InvalidCandidateProfileException(string message)
            : base(string.IsNullOrEmpty(message) ? "The skill is not valid" : message)
        {
        }
    }

    public class DeleteCandidateProfileNotFoundException : InvalidCandidateProfileException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.DeleteCandidateProfileNotFound;

        public DeleteCandidateProfileNotFoundException(int profileId)
            : base($"Profile not found for the Profile Id: {profileId}")
        {
            ProfileId = profileId;
        }

        public int ProfileId { get; set; }
    }

    public class CandidateProfileDeletedException : InvalidCandidateProfileException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.CandidateProfileDeleted;

        public CandidateProfileDeletedException(int id, string name)
            : base($"The profile {name} was deleted")
        {
            ProfileId = id;
            Name = name;
        }

        public int ProfileId { get; set; }
        public string Name { get; set; }
    }
    public class InvalidUpdateException : InvalidCandidateProfileException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the profile.")
        {
        }
    }

    public class UpdateCandidateProfileNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.UpdateCandidateProfileNotFound;

        public UpdateCandidateProfileNotFoundException(int profileId, Guid clientSystemId)
            : base($"Profile {profileId} and Client System Id {clientSystemId} was not found.")
        {
            ProfileId = profileId;
            ClientSystemId = clientSystemId;
        }
  
        public int ProfileId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int profileId, Guid clientSystemId, string name)
            : base($"Profile {name} has not changes.")
        {
            ProfileId = profileId;
            ClientSystemId = clientSystemId;
        }

        public int ProfileId { get; }
        public Guid ClientSystemId { get; }
    }

    public class CandidateProfileNotFoundException : InvalidCandidateProfileException
    {
        protected override int SubErrorCode => (int)CandidateProfileErrorSubCodes.CandidateProfileNotFound;

        public CandidateProfileNotFoundException(int profileId) : base($"The Profile {profileId} was not found.")
        {
            ProfileId = profileId;
        }

        public int ProfileId { get; }
    }
}

