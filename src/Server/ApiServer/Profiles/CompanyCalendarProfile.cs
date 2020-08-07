// <copyright file="CompanyCalendarProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.CompanyCalendar;
    using AutoMapper;
    using Domain.Services.Contracts.CompanyCalendar;

    public class CompanyCalendarProfile : Profile
    {
        public CompanyCalendarProfile()
        {
            this.CreateMap<CreateCompanyCalendarViewModel, CreateCompanyCalendarContract>();
            this.CreateMap<CreatedCompanyCalendarContract, CreatedCompanyCalendarViewModel>();
            this.CreateMap<ReadedCompanyCalendarContract, ReadedCompanyCalendarViewModel>();
            this.CreateMap<UpdateCompanyCalendarViewModel, UpdateCompanyCalendarContract>();
        }
    }
}
