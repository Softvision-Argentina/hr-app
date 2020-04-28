using Domain.Services.Contracts.CompanyCalendar;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ICompanyCalendarService
    {
        CreatedCompanyCalendarContract Create(CreateCompanyCalendarContract contract);
        ReadedCompanyCalendarContract Read(int id);
        void Update(UpdateCompanyCalendarContract contract);
        void Delete(int id);
        IEnumerable<ReadedCompanyCalendarContract> List();
    }
}
