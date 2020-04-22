using ApiServer.Contracts.CompanyCalendar;
using AutoMapper;
using Domain.Services.Contracts.CompanyCalendar;

namespace ApiServer.Profiles
{
    public class CompanyCalendarProfile : Profile
    {
        public CompanyCalendarProfile()
        {
            CreateMap<CreateCompanyCalendarViewModel, CreateCompanyCalendarContract>();
            CreateMap<CreatedCompanyCalendarContract, CreatedCompanyCalendarViewModel>();
            CreateMap<ReadedCompanyCalendarContract, ReadedCompanyCalendarViewModel>();
            CreateMap<UpdateCompanyCalendarViewModel, UpdateCompanyCalendarContract>();
        }

    }
}
