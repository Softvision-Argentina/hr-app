// <copyright file="EmployeeCasualtyProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.EmployeeCasualty;

    public class EmployeeCasualtyProfile : Profile
    {
        public EmployeeCasualtyProfile()
        {
            this.CreateMap<EmployeeCasualty, ReadedEmployeeCasualtyContract>();
            this.CreateMap<CreateEmployeeCasualtyContract, EmployeeCasualty>();
            this.CreateMap<EmployeeCasualty, CreatedEmployeeCasualtyContract>();
            this.CreateMap<UpdateEmployeeCasualtyContract, EmployeeCasualty>();
        }
    }
}
