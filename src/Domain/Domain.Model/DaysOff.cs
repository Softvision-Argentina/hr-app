using Core;
using Domain.Model.Enum;
using System;

namespace Domain.Model
{
    public class DaysOff : Entity<int>
    {
        public DaysOffStatus Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public DaysOffType Type { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string GoogleCalendarEventId { get; set; }
    }
}
