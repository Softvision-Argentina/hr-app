using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.UserDashboard;

namespace Domain.Services.Impl.Profiles
{
    public class UserDashboardProfile : Profile
    {
        public UserDashboardProfile()
        {
            CreateMap<UserDashboard, ReadedUserDashboardContract>();
            CreateMap<CreateUserDashboardContract, UserDashboard>();
            CreateMap<UserDashboard, CreatedUserDashboardContract>();
            CreateMap<UpdateUserDashboardContract, UserDashboard>();
        }
    }
}
