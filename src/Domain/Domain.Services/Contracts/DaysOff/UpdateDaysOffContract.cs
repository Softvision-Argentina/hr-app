using Domain.Model.Enum;
using Domain.Services.Contracts.Employee;
using System;

namespace Domain.Services.Contracts.DaysOff
{
    public class UpdateDaysOffContract
    {
        public int Id { get; set; }
        public DaysOffStatus Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public DaysOffType Type { get; set; }
        public int EmployeeId { get; set; }
        public UpdateEmployeeContract Employee { get; set; }
    }
}
