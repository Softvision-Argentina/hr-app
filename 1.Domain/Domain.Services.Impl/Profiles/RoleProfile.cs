using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Role;

namespace Domain.Services.Impl.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, ReadedRoleContract>();
            CreateMap<CreateRoleContract, Role>();
            CreateMap<Role, CreatedRoleContract>();
            CreateMap<UpdateRoleContract, Role>();
        }
    }
}
