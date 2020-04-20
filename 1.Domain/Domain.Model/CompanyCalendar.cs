using Core;
using System;

namespace Domain.Model
{
    public class CompanyCalendar : Entity<int>
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
