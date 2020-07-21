using System;

namespace ApiServer.Contracts.CompanyCalendar
{
    public class CreateCompanyCalendarViewModel
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
