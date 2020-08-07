// <copyright file="HireProjectionProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.HireProjection;
    using AutoMapper;
    using Domain.Services.Contracts.HireProjection;

    public class HireProjectionProfile : Profile
    {
        public HireProjectionProfile()
        {
            this.CreateMap<CreateHireProjectionViewModel, CreateHireProjectionContract>();
            this.CreateMap<CreatedHireProjectionContract, CreatedHireProjectionViewModel>();
            this.CreateMap<ReadedHireProjectionContract, ReadedHireProjectionViewModel>();
            this.CreateMap<UpdateHireProjectionViewModel, UpdateHireProjectionContract>();
        }
    }
}
