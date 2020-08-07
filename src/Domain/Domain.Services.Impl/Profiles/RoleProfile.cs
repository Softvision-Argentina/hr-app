// <copyright file="RoleProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Role;

    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            this.CreateMap<Role, ReadedRoleContract>();
            this.CreateMap<CreateRoleContract, Role>();
            this.CreateMap<Role, CreatedRoleContract>();
            this.CreateMap<UpdateRoleContract, Role>();
        }
    }
}
