// <copyright file="EmployeeCasualtyProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.EmployeeCasualty;
    using AutoMapper;
    using Domain.Services.Contracts.EmployeeCasualty;

    public class EmployeeCasualtyProfile : Profile
    {
        public EmployeeCasualtyProfile()
        {
            this.CreateMap<CreateEmployeeCasualtyViewModel, CreateEmployeeCasualtyContract>();
            this.CreateMap<CreatedEmployeeCasualtyContract, CreatedEmployeeCasualtyViewModel>();
            this.CreateMap<ReadedEmployeeCasualtyContract, ReadedEmployeeCasualtyViewModel>();
            this.CreateMap<UpdateEmployeeCasualtyViewModel, UpdateEmployeeCasualtyContract>();
        }
    }
}
