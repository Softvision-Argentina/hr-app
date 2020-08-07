// <copyright file="UserDashboardProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.UserDashboard;
    using AutoMapper;
    using Domain.Services.Contracts.UserDashboard;

    public class UserDashboardProfile : Profile
    {
        public UserDashboardProfile()
        {
            this.CreateMap<CreateUserDashboardViewModel, CreateUserDashboardContract>();
            this.CreateMap<CreatedUserDashboardContract, CreatedUserDashboardViewModel>();
            this.CreateMap<ReadedUserDashboardContract, ReadedUserDashboardViewModel>();
            this.CreateMap<UpdateUserDashboardViewModel, UpdateUserDashboardContract>();
        }
    }
}
