// <copyright file="DashboardProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Dashboard;
    using AutoMapper;
    using Domain.Services.Contracts.Dashboard;

    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            this.CreateMap<CreateDashboardViewModel, CreateDashboardContract>();
            this.CreateMap<CreatedDashboardContract, CreatedDashboardViewModel>();
            this.CreateMap<ReadedDashboardContract, ReadedDashboardViewModel>();
            this.CreateMap<UpdateDashboardViewModel, UpdateDashboardContract>();
        }
    }
}
