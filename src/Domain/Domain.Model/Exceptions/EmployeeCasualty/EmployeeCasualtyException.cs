using Core;
using System;

namespace Domain.Model.Exceptions.EmployeeCasualty
{
    public class EmployeeCasualtyException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.EmployeeCasualty;

        public EmployeeCasualtyException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a employee casualty related error" : message)
        {
        }
    }

    public class InvalidEmployeeCasualtyException : EmployeeCasualtyException
    {
        public InvalidEmployeeCasualtyException(string message)
            : base(string.IsNullOrEmpty(message) ? "The employee casualty is not valid" : message)
        {
        }
    }

    public class DeleteEmployeeCasualtyNotFoundException : InvalidEmployeeCasualtyException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.DeleteEmployeeCasualtyNotFound;

        public DeleteEmployeeCasualtyNotFoundException(int employeeCasualtyId)
            : base($"Employee casualty not found for the EmployeeCasualtyId: {employeeCasualtyId}")
        {
            EmployeeCasualtyId = employeeCasualtyId;
        }

        public int EmployeeCasualtyId { get; set; }
    }

    public class EmployeeCasualtyDeletedException : InvalidEmployeeCasualtyException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.EmployeeCasualtyDeleted;

        public EmployeeCasualtyDeletedException(int id, int month, int year)
            : base($"The employee casualty {year}-{month} was deleted")
        {
            EmployeeCasualtyId = id;
            Month = month;
            Year = year;
        }

        public int EmployeeCasualtyId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    public class InvalidUpdateException : InvalidEmployeeCasualtyException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the employee casualty.")
        {
        }
    }

    public class UpdateSkillNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.UpdateEmployeeCasualtyNotFound;

        public UpdateSkillNotFoundException(int employeeCasualtyId, Guid clientSystemId)
            : base($"skill {employeeCasualtyId} and Client System Id {clientSystemId} was not found.")
        {
            EmployeeCasualtyId = employeeCasualtyId;
            ClientSystemId = clientSystemId;
        }

        public int EmployeeCasualtyId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int skillId, Guid clientSystemId, int month, int year)
            : base($"Employee casualty {year}-{month} has not changes.")
        {
            EmployeeCasualtyId = skillId;
            ClientSystemId = clientSystemId;
        }

        public int EmployeeCasualtyId { get; }
        public Guid ClientSystemId { get; }
    }

    public class EmployeeCasualtyNotFoundException : InvalidEmployeeCasualtyException
    {
        protected override int SubErrorCode => (int)EmployeeCasualtyErrorSubCodes.EmployeeCasualtyNotFound;

        public EmployeeCasualtyNotFoundException(int employeeCasualtyId) : base($"The employee casualty {employeeCasualtyId} was not found.")
        {
            EmployeeCasualtyId = employeeCasualtyId;
        }

        public int EmployeeCasualtyId { get; }
    }
}
