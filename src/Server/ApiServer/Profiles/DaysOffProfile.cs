// <copyright file="DaysOffProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.DaysOff;
    using AutoMapper;
    using Domain.Services.Contracts.DaysOff;

    public class DaysOffProfile : Profile
    {
        public DaysOffProfile()
        {
            this.CreateMap<CreateDaysOffViewModel, CreateDaysOffContract>();
            this.CreateMap<CreatedDaysOffContract, CreatedDaysOffViewModel>();
            this.CreateMap<ReadedDaysOffContract, ReadedDaysOffViewModel>();
            this.CreateMap<UpdateDaysOffViewModel, UpdateDaysOffContract>();
        }
    }
}
