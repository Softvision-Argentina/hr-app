// <copyright file="OpenPositionProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.OpenPositions;

    public class OpenPositionProfile : Profile
    {
        public OpenPositionProfile()
        {
            this.CreateMap<OpenPosition, ReadedOpenPositionContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            this.CreateMap<CreateOpenPositionContract, OpenPosition>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            this.CreateMap<OpenPosition, CreateOpenPositionContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            this.CreateMap<OpenPosition, CreatedOpenPositionContract>();

            this.CreateMap<UpdateOpenPositionContract, OpenPosition>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));
        }
    }
}
