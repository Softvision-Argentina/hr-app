// <copyright file="DaysOffProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.DaysOff;

    public class DaysOffProfile : Profile
    {
        public DaysOffProfile()
        {
            this.CreateMap<DaysOff, ReadedDaysOffContract>();
            this.CreateMap<CreateDaysOffContract, DaysOff>();
            this.CreateMap<DaysOff, CreatedDaysOffContract>();
            this.CreateMap<UpdateDaysOffContract, DaysOff>();
        }
    }
}
