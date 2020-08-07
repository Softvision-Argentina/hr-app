// <copyright file="CommunityProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Community;
    using AutoMapper;
    using Domain.Services.Contracts.Community;

    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            this.CreateMap<CreateCommunityViewModel, CreateCommunityContract>();
            this.CreateMap<CreatedCommunityContract, CreatedCommunityViewModel>();
            this.CreateMap<ReadedCommunityContract, ReadedCommunityViewModel>();
            this.CreateMap<UpdateCommunityViewModel, UpdateCommunityContract>();
        }
    }
}
