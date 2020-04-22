using ApiServer.Contracts.Skills;
using AutoMapper;
using Domain.Services.Contracts.Skill;

namespace ApiServer.Profiles
{
    public class SkillProfile: Profile
    {
        public SkillProfile()
        {
            CreateMap<CreateSkillViewModel, CreateSkillContract>();
            CreateMap<CreatedSkillContract, CreatedSkillViewModel>();
            CreateMap<ReadedSkillContract, ReadedSkillViewModel>();
            CreateMap<UpdateSkillViewModel, UpdateSkillContract>();
        }
    }
}
