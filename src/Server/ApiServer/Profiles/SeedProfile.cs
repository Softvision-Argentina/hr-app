// <copyright file="SeedProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Seed;
    using AutoMapper;
    using Domain.Services.Contracts.Seed;

    public class SeedProfile : Profile
    {
        public SeedProfile()
        {
            this.CreateMap<CreateDummyViewModel, CreateDummyViewModel>();
            this.CreateMap<CreatedDummyContract, CreatedDummyViewModel>();
            this.CreateMap<ReadedDummyContract, ReadedDummyViewModel>();
            this.CreateMap<UpdateDummyViewModel, UpdateDummyContract>();
        }
    }
}
