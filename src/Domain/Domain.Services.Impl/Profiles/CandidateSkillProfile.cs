// <copyright file="CandidateSkillProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.CandidateSkill;

    public class CandidateSkillProfile : Profile
    {
        public CandidateSkillProfile()
        {
            this.CreateMap<CandidateSkill, ReadedCandidateSkillContract>();
            this.CreateMap<CreateCandidateSkillContract, CandidateSkill>();
            this.CreateMap<CandidateSkill, CreatedCandidateSkillContract>();
            this.CreateMap<UpdateCandidateSkillContract, CandidateSkill>();
        }
    }
}
