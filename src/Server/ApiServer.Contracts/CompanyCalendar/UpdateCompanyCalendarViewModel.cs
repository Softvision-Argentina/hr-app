using System;

namespace ApiServer.Contracts.CompanyCalendar
{
    public class UpdateCompanyCalendarViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
