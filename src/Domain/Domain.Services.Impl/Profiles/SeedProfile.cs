// <copyright file="SeedProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model.Seed;
    using Domain.Services.Contracts.Seed;

    public class SeedProfile : Profile
    {
        public SeedProfile()
        {
            this.CreateMap<Dummy, ReadedDummyContract>();
            this.CreateMap<CreateDummyContract, Dummy>();
            this.CreateMap<Dummy, CreatedDummyContract>();
            this.CreateMap<UpdateDummyContract, Dummy>();
        }
    }
}
