// <copyright file="DashboardProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Dashboard;

    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            this.CreateMap<Dashboard, ReadedDashboardContract>();
            this.CreateMap<CreateDashboardContract, Dashboard>();
            this.CreateMap<Dashboard, CreatedDashboardContract>();
            this.CreateMap<UpdateDashboardContract, Dashboard>();
        }
    }
}
