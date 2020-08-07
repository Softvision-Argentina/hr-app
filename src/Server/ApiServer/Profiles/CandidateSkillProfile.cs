// <copyright file="CandidateSkillProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.CandidateSkill;
    using AutoMapper;
    using Domain.Services.Contracts.CandidateSkill;

    public class CandidateSkillProfile : Profile
    {
        public CandidateSkillProfile()
        {
            this.CreateMap<CreateCandidateSkillViewModel, CreateCandidateSkillContract>();
            this.CreateMap<CreatedCandidateSkillContract, CreatedCandidateSkillViewModel>();
            this.CreateMap<ReadedCandidateSkillContract, ReadedCandidateSkillViewModel>();
            this.CreateMap<ReadedCandidateAppSkillContract, ReadedCandidateAppSkillViewModel>();
            this.CreateMap<UpdateCandidateSkillViewModel, UpdateCandidateSkillContract>();
        }
    }
}
