using ApiServer.Contracts.UserDashboard;
using Domain.Services.Contracts.UserDashboard;
using AutoMapper;

namespace ApiServer.Profiles
{
    public class UserDashboardProfile : Profile
    {
        public UserDashboardProfile()
        {
            CreateMap<CreateUserDashboardViewModel, CreateUserDashboardContract>();
            CreateMap<CreatedUserDashboardContract, CreatedUserDashboardViewModel>();
            CreateMap<ReadedUserDashboardContract, ReadedUserDashboardViewModel>();
            CreateMap<UpdateUserDashboardViewModel, UpdateUserDashboardContract>();
        }
    }
}
