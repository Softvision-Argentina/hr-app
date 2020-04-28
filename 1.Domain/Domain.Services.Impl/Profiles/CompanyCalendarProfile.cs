using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.CompanyCalendar;

namespace Domain.Services.Impl.Profiles
{
    class CompanyCalendarProfile : Profile
    {
        public CompanyCalendarProfile()
        {
            CreateMap<CompanyCalendar, ReadedCompanyCalendarContract>();
            CreateMap<CreateCompanyCalendarContract, CompanyCalendar>();
            CreateMap<CompanyCalendar, CreatedCompanyCalendarContract>();
            CreateMap<UpdateCompanyCalendarContract, CompanyCalendar>();
        }
    }
}
