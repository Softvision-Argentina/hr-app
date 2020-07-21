using System;

namespace Domain.Services.Contracts.CompanyCalendar
{
    public class UpdateCompanyCalendarContract
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
