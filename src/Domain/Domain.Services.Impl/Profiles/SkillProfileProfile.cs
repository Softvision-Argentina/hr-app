namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Services.Contracts.ProfileByCommunity;
    using Domain.Services.Contracts.SkillProfile;

    public class SkillProfileProfile : Profile
    {
        public SkillProfileProfile()
        {
            this.CreateMap<Model.SkillProfile, ReadedSkillProfileContract>()
                .ForMember(x => x.Skill, opt => opt.MapFrom(r => r.Skill))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<CreateSkillProfileContract, Model.SkillProfile>()
                .ForMember(x => x.Skill, opt => opt.Ignore())
                .ForMember(x => x.Profile, opt => opt.Ignore());
            this.CreateMap<Model.SkillProfile, CreateSkillProfileContract>()
                .ForMember(x => x.Skill, opt => opt.MapFrom(r => r.Skill))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<Model.SkillProfile, CreatedSkillProfileContract>();
            this.CreateMap<UpdateSkillProfileContract, Model.SkillProfile>()
                .ForMember(x => x.Skill, opt => opt.Ignore())
                .ForMember(x => x.Profile, opt => opt.Ignore());
            this.CreateMap<CreateSkillProfileViewModel, CreateSkillProfileContract>();
            this.CreateMap<CreatedSkillProfileContract, CreatedSkillProfileViewModel>();
            this.CreateMap<ReadedSkillProfileContract, ReadedSkillProfileViewModel>()
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<UpdateSkillProfileViewModel, UpdateSkillProfileContract>()
                .ForMember(x => x.Skill, opt => opt.MapFrom(r => r.Skill))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
        }
    }
}
