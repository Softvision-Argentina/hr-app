// <copyright file="UserProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.User;
    using AutoMapper;
    using Domain.Services.Contracts.User;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<CreateUserViewModel, CreateUserContract>();
            this.CreateMap<CreatedUserContract, CreatedUserViewModel>();
            this.CreateMap<ReadedUserContract, ReadedUserViewModel>();
            this.CreateMap<UpdateUserViewModel, UpdateUserContract>();
            this.CreateMap<ReadedUserRoleContract, ReadedUserRoleViewModel>();
            this.CreateMap<ReadedUserViewModel, ReadedUserContract>();
        }
    }
}
