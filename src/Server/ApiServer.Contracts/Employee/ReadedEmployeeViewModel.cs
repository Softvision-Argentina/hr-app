using ApiServer.Contracts.Role;
using Domain.Model.Enum;

namespace ApiServer.Contracts.Employee
{
    public class ReadedEmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DNI { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string LinkedInProfile { get; set; }
        public string AdditionalInformation { get; set; }
        public EmployeeStatus Status { get; set; }
        public int UserId { get; set; }
        public ReadedRoleViewModel Role { get; set; }
        public bool isReviewer { get; set; }
        public ReadedEmployeeViewModel Reviewer { get; set; }
    }
}
