using ApiServer.Contracts.Employee;
using Domain.Model.Enum;
using System;

namespace ApiServer.Contracts.DaysOff
{
    public class UpdateDaysOffViewModel
    {
        public int Id { get; set; }
        public DaysOffStatus Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public DaysOffType Type { get; set; }
        public int EmployeeId { get; set; }
        public UpdateEmployeeViewModel Employee { get; set; }
    }
}
