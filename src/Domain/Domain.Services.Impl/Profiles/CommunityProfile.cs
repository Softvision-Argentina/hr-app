// <copyright file="CommunityProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using Domain.Model;
    using Domain.Services.Contracts.Community;

    public class CommunityProfile : AutoMapper.Profile
    {
        public CommunityProfile()
        {
            this.CreateMap<Community, ReadedCommunityContract>();
            this.CreateMap<ReadedCommunityContract, Community>();
            this.CreateMap<CreateCommunityContract, Community>();
            this.CreateMap<Community, CreatedCommunityContract>();
            this.CreateMap<UpdateCommunityContract, Community>();
        }
    }
}
