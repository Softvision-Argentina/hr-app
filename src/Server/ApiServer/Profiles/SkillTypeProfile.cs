// <copyright file="SkillTypeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.SkillType;
    using AutoMapper;
    using Domain.Services.Contracts.SkillType;

    public class SkillTypeProfile : Profile
    {
        public SkillTypeProfile()
        {
            this.CreateMap<CreateSkillTypeViewModel, CreateSkillTypeContract>();
            this.CreateMap<CreatedSkillTypeContract, CreatedSkillTypeViewModel>();
            this.CreateMap<ReadedSkillTypeContract, ReadedSkillTypeViewModel>();
            this.CreateMap<UpdateSkillTypeViewModel, UpdateSkillTypeContract>();
        }
    }
}
