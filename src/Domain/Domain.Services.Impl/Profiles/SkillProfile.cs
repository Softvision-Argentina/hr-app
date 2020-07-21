using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Skill;

namespace Domain.Services.Impl.Profiles
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, ReadedSkillContract>().ForMember(x => x.Type, opt => opt.MapFrom(s => s.Type.Id));
            CreateMap<CreateSkillContract, Skill>().ForMember(x => x.Type, opt => opt.Ignore());
            CreateMap<Skill, CreatedSkillContract>();
            CreateMap<UpdateSkillContract, Skill>().ForMember(x => x.Type, opt => opt.Ignore());
        }
    }
}
