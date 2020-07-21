using System;

namespace Domain.Services.Contracts.CompanyCalendar
{
    public class CreateCompanyCalendarContract
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
