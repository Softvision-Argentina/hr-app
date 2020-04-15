using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Exceptions.Employee
{
    public class EmployeeException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Employee;

        public EmployeeException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a employee related error" : message)
        {
        }
    }

    public class InvalidEmployeeException : EmployeeException
    {
        public InvalidEmployeeException(string message)
            : base(string.IsNullOrEmpty(message) ? "The User is not valid" : message)
        {
        }
    }

    public class InvalidUpdateException : InvalidEmployeeException
    {
        protected override int SubErrorCode => (int)EmployeeErrorSubCodes.InvalidUpdate;
        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the employee.")
        {
        }
    }

    public class DeleteEmployeeNotFoundException : InvalidEmployeeException
    {
        protected override int SubErrorCode => (int)EmployeeErrorSubCodes.DeleteEmployeeNotFound;
        public DeleteEmployeeNotFoundException(int employeeId)
            : base($"Employee not found for the UserId: {employeeId}")
        {
            EmployeeId = employeeId;
        }

        public int EmployeeId { get; set; }
    }

    public class EmployeeDeletedException : InvalidEmployeeException
    {
        protected override int SubErrorCode => (int)EmployeeErrorSubCodes.EmployeeDeleted;
        public EmployeeDeletedException(int id, string name)
            : base($"The employee {name} was deleted")
        {
            EmployeeId = id;
            Name = name;
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateEmployeeNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)EmployeeErrorSubCodes.UpdateEmployeeNotFound;
        public UpdateEmployeeNotFoundException(int employeeId, Guid clientSystemId)
            : base($"User {employeeId} and Client System Id {clientSystemId} was not found.")
        {
            EmployeeId = employeeId;
            ClientSystemId = clientSystemId;
        }

        public int EmployeeId { get; }
        public Guid ClientSystemId { get; }
    }

    public class EmployeeNotFoundException : InvalidEmployeeException
    {
        protected override int SubErrorCode => (int)EmployeeErrorSubCodes.EmployeeNotFound;
        public EmployeeNotFoundException(int employeeId) : base($"The Employee {employeeId} was not found.")
        {
            EmployeeId = employeeId;
        }

        public int EmployeeId { get; }
    }

}

