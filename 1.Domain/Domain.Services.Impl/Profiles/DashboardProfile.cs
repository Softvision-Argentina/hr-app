using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.Dashboard;
using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Dashboard;

namespace Domain.Services.Impl.Profiles
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            CreateMap<Dashboard, ReadedDashboardContract>();
            CreateMap<CreateDashboardContract, Dashboard>();
            CreateMap<Dashboard, CreatedDashboardContract>();
            CreateMap<UpdateDashboardContract, Dashboard>();
            CreateMap<UpdateDashboardViewModel, UpdateDashboardContract>();
        }
    }
}
