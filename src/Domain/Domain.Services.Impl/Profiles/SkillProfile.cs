// <copyright file="SkillProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Skill;

    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            this.CreateMap<Skill, ReadedSkillContract>().ForMember(x => x.Type, opt => opt.MapFrom(s => s.Type.Id));
            this.CreateMap<CreateSkillContract, Skill>().ForMember(x => x.Type, opt => opt.Ignore());
            this.CreateMap<Skill, CreatedSkillContract>();
            this.CreateMap<UpdateSkillContract, Skill>().ForMember(x => x.Type, opt => opt.Ignore());
        }
    }
}
