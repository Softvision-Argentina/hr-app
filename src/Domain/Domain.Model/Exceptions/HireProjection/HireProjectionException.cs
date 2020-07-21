using Core;
using System;

namespace Domain.Model.Exceptions.HireProjection
{
    public class HireProjectionException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.HireProjection;

        public HireProjectionException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a hire projection related error" : message)
        {
        }
    }

    public class InvalidHireProjectionException : HireProjectionException
    {
        public InvalidHireProjectionException(string message)
            : base(string.IsNullOrEmpty(message) ? "The hire projection is not valid" : message)
        {
        }
    }

    public class DeleteHireProjectionNotFoundException : InvalidHireProjectionException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.DeleteHireProjectionNotFound;

        public DeleteHireProjectionNotFoundException(int hireProjectionId)
            : base($"Hire projection not found for the hireProjectionId: {hireProjectionId}")
        {
            HireProjectionId = hireProjectionId;
        }

        public int HireProjectionId { get; set; }
    }

    public class HireProjectionDeletedException : InvalidHireProjectionException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.HireProjectionDeleted;

        public HireProjectionDeletedException(int id, int month, int year)
            : base($"The hire projection {year}-{month} was deleted")
        {
            HireProjectionId = id;
            Month = month;
            Year = year;
        }

        public int HireProjectionId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class InvalidUpdateException : InvalidHireProjectionException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the hire projection.")
        {
        }
    }

    public class UpdateSkillNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.UpdateHireProjectionNotFound;
        public UpdateSkillNotFoundException(int hireProjectionId, Guid clientSystemId)
            : base($"skill {hireProjectionId} and Client System Id {clientSystemId} was not found.")
        {
            HireProjectionId = hireProjectionId;
            ClientSystemId = clientSystemId;
        }

        public int HireProjectionId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int skillId, Guid clientSystemId, int month, int year)
            : base($"Hire projection {year}-{month} has not changes.")
        {
            HireProjectionId = skillId;
            ClientSystemId = clientSystemId;
        }

        public int HireProjectionId { get; }
        public Guid ClientSystemId { get; }
    }

    public class HireProjectionNotFoundException : InvalidHireProjectionException
    {
        protected override int SubErrorCode => (int)HireProjectionErrorSubCodes.HireProjectionNotFound;

        public HireProjectionNotFoundException(int hireProjectionId) : base($"The hire projection {hireProjectionId} was not found.")
        {
            HireProjectionId = hireProjectionId;
        }

        public int HireProjectionId { get; }
    }
}
