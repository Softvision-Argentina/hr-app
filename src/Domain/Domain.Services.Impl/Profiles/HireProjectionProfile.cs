// <copyright file="HireProjectionProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.HireProjection;

    public class HireProjectionProfile : Profile
    {
        public HireProjectionProfile()
        {
            this.CreateMap<HireProjection, ReadedHireProjectionContract>();
            this.CreateMap<CreateHireProjectionContract, HireProjection>();
            this.CreateMap<HireProjection, CreatedHireProjectionContract>();
            this.CreateMap<UpdateHireProjectionContract, HireProjection>();
        }
    }
}
