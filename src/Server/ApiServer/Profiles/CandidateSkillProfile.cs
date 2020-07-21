using ApiServer.Contracts.CandidateSkill;
using AutoMapper;
using Domain.Services.Contracts.CandidateSkill;

namespace ApiServer.Profiles
{
    public class CandidateSkillProfile: Profile
    {
        public CandidateSkillProfile()
        {
            CreateMap<CreateCandidateSkillViewModel, CreateCandidateSkillContract>();
            CreateMap<CreatedCandidateSkillContract, CreatedCandidateSkillViewModel>();
            CreateMap<ReadedCandidateSkillContract, ReadedCandidateSkillViewModel>();
            CreateMap<ReadedCandidateAppSkillContract, ReadedCandidateAppSkillViewModel>();
            CreateMap<UpdateCandidateSkillViewModel, UpdateCandidateSkillContract>();
        }
    }
}
