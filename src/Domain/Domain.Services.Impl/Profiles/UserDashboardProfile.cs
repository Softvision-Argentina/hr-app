// <copyright file="UserDashboardProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.UserDashboard;

    public class UserDashboardProfile : Profile
    {
        public UserDashboardProfile()
        {
            this.CreateMap<UserDashboard, ReadedUserDashboardContract>();
            this.CreateMap<CreateUserDashboardContract, UserDashboard>();
            this.CreateMap<UserDashboard, CreatedUserDashboardContract>();
            this.CreateMap<UpdateUserDashboardContract, UserDashboard>();
        }
    }
}
