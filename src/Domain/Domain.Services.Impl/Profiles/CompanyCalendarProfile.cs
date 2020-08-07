// <copyright file="CompanyCalendarProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.CompanyCalendar;

    internal class CompanyCalendarProfile : Profile
    {
        public CompanyCalendarProfile()
        {
            this.CreateMap<CompanyCalendar, ReadedCompanyCalendarContract>();
            this.CreateMap<CreateCompanyCalendarContract, CompanyCalendar>();
            this.CreateMap<CompanyCalendar, CreatedCompanyCalendarContract>();
            this.CreateMap<UpdateCompanyCalendarContract, CompanyCalendar>();
        }
    }
}
