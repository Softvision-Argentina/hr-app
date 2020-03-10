using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
