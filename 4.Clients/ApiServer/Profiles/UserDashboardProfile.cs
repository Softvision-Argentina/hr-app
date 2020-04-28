using ApiServer.Contracts.UserDashboard;
using AutoMapper;
using Domain.Services.Contracts.UserDashboard;

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
