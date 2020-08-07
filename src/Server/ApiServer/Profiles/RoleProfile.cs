// <copyright file="RoleProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Role;
    using AutoMapper;
    using Domain.Services.Contracts.Role;

    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            this.CreateMap<CreateRoleViewModel, CreateRoleContract>();
            this.CreateMap<ReadedRoleContract, ReadedRoleViewModel>();
            this.CreateMap<UpdateRoleViewModel, UpdateRoleContract>();
            this.CreateMap<CreatedRoleContract, CreatedRoleViewModel>();
        }
    }
}
