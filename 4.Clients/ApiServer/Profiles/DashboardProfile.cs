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
