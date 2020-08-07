// <copyright file="SkillTypeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.SkillType;

    public class SkillTypeProfile : Profile
    {
        public SkillTypeProfile()
        {
            this.CreateMap<SkillType, ReadedSkillTypeContract>();
            this.CreateMap<ReadedSkillTypeContract, SkillType>();
            this.CreateMap<CreateSkillTypeContract, SkillType>();
            this.CreateMap<SkillType, CreatedSkillTypeContract>();
            this.CreateMap<UpdateSkillTypeContract, SkillType>();
        }
    }
}
