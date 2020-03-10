using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Dashboard;
using AutoMapper;
using Domain.Services.Contracts.Dashboard;

namespace ApiServer.Profiles
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            CreateMap<CreateDashboardViewModel, CreateDashboardContract>();
            CreateMap<CreatedDashboardContract, CreatedDashboardViewModel>();
            CreateMap<ReadedDashboardContract, ReadedDashboardViewModel>();
            CreateMap<UpdateDashboardViewModel, UpdateDashboardContract>();
        }
    }
}
