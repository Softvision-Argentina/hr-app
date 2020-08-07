// <copyright file="UserProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.User;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, ReadedUserContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));
            this.CreateMap<ReadedUserContract, User>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));
            this.CreateMap<CreateUserContract, User>();
            this.CreateMap<User, CreatedUserContract>();
            this.CreateMap<UpdateUserContract, User>();
        }
    }
}
