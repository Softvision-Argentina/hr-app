using ApiServer.Contracts.Role;
using ApiServer.Contracts.User;
using Domain.Model.Enum;

namespace ApiServer.Contracts.Employee
{
    public class UpdateEmployeeViewModel
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
        public CreateUserViewModel User { get; set; }
        public int RoleId { get; set; }
        public CreateRoleViewModel Role { get; set; }
        // TODO: Rename.
        public bool isReviewer { get; set; }
        public int? ReviewerId { get; set; }
        public CreateEmployeeViewModel Reviewer { get; set; }
    }
}
