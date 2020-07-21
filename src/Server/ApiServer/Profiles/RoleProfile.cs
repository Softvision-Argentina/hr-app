using ApiServer.Contracts.Role;
using AutoMapper;
using Domain.Services.Contracts.Role;

namespace ApiServer.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleViewModel, CreateRoleContract>();
            CreateMap<ReadedRoleContract, ReadedRoleViewModel>();
            CreateMap<UpdateRoleViewModel, UpdateRoleContract>();
            CreateMap<CreatedRoleContract, CreatedRoleViewModel>();
        }
    }
}
