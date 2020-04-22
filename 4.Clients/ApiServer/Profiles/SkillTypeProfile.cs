using ApiServer.Contracts.SkillType;
using AutoMapper;
using Domain.Services.Contracts.SkillType;

namespace ApiServer.Profiles
{
    public class SkillTypeProfile : Profile
    {
        public SkillTypeProfile()
        {
            CreateMap<CreateSkillTypeViewModel, CreateSkillTypeContract>();
            CreateMap<CreatedSkillTypeContract, CreatedSkillTypeViewModel>();
            CreateMap<ReadedSkillTypeContract, ReadedSkillTypeViewModel>();
            CreateMap<UpdateSkillTypeViewModel, UpdateSkillTypeContract>();
        }
    }
}
