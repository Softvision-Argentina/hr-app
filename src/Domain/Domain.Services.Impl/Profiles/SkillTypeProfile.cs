using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.SkillType;

namespace Domain.Services.Impl.Profiles
{
    public class SkillTypeProfile : Profile
    {
        public SkillTypeProfile()
        {
            CreateMap<SkillType, ReadedSkillTypeContract>();
            CreateMap<ReadedSkillTypeContract, SkillType>();
            CreateMap<CreateSkillTypeContract, SkillType>();
            CreateMap<SkillType, CreatedSkillTypeContract>();
            CreateMap<UpdateSkillTypeContract, SkillType>();
        }
    }
}
