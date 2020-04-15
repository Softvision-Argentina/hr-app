using ApiServer.Contracts.User;
using AutoMapper;
using Domain.Services.Contracts.User;

namespace ApiServer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserViewModel, CreateUserContract>();
            CreateMap<CreatedUserContract, CreatedUserViewModel>();
            CreateMap<ReadedUserContract, ReadedUserViewModel>();
            CreateMap<UpdateUserViewModel, UpdateUserContract>();
            CreateMap<ReadedUserRoleContract, ReadedUserRoleViewModel>();
            CreateMap<ReadedUserViewModel,ReadedUserContract>();
        }
    }
}