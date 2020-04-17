using Core;
using System;

namespace Domain.Model.Exceptions.CompanyCalendar
{
    public class CompanyCalendarException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.CompanyCalendar;

        public CompanyCalendarException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a company calendar related error" : message)
        {
        }
    }

    public class InvalidCompanyCalendarException : CompanyCalendarException
    {
        public InvalidCompanyCalendarException(string message)
            : base(string.IsNullOrEmpty(message) ? "The company calendar is not valid" : message)
        {
        }
    }

    public class DeleteCompanyCalendarNotFoundException : InvalidCompanyCalendarException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.DeleteCompanyCalendarNotFound;

        public DeleteCompanyCalendarNotFoundException(int companyCalendarId)
            : base($"Company calendar not found for the Company calendar Id: {companyCalendarId}")
        {
            CompanyCalendarId = companyCalendarId;
        }

        public int CompanyCalendarId { get; set; }
    }

    public class CompanyCalendarDeletedException : InvalidCompanyCalendarException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.CompanyCalendarDeleted;

        public CompanyCalendarDeletedException(int id, string name)
            : base($"The company calendar {name} was deleted")
        {
            CompanyCalendarId = id;
            Name = name;
        }

        public int CompanyCalendarId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidCompanyCalendarException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the company calendar")
        {
        }
    }

    public class UpdateCompanyCalendarNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.UpdateCompanyCalendarNotFound;

        public UpdateCompanyCalendarNotFoundException(int companyCalendarId, Guid clientSystemId)
            : base($"Profile {companyCalendarId} and Client System Id {clientSystemId} was not found.")
        {
            CompanyCalendarId = companyCalendarId;
            ClientSystemId = clientSystemId;
        }
  
        public int CompanyCalendarId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.UpdateHasNotChanges;
        public UpdateHasNotChangesException(int companyCalendarId, Guid clientSystemId, string name)
            : base($"Profile {name} has not changes.")
        {
            CompanyCalendarId = companyCalendarId;
            ClientSystemId = clientSystemId;
        }

        public int CompanyCalendarId { get; }
        public Guid ClientSystemId { get; }
    }

    public class CompanyCalendarNotFoundException : InvalidCompanyCalendarException
    {
        protected override int SubErrorCode => (int)CompanyCalendarErrorSubCodes.CompanyCalendarNotFound;

        public CompanyCalendarNotFoundException(int companyCalendarId) : base($"The Profile {companyCalendarId} was not found.")
        {
            CompanyCalendarId = companyCalendarId;
        }

        public int CompanyCalendarId { get; }
    }
}
