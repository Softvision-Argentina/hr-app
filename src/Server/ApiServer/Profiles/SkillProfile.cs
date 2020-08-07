// <copyright file="SkillProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Skills;
    using AutoMapper;
    using Domain.Services.Contracts.Skill;

    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            this.CreateMap<CreateSkillViewModel, CreateSkillContract>();
            this.CreateMap<CreatedSkillContract, CreatedSkillViewModel>();
            this.CreateMap<ReadedSkillContract, ReadedSkillViewModel>();
            this.CreateMap<UpdateSkillViewModel, UpdateSkillContract>();
        }
    }
}
