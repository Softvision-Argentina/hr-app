using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.CandidateSkill;

namespace Domain.Services.Impl.Profiles
{
    public class CandidateSkillProfile: Profile
    {
        public CandidateSkillProfile()
        {
            CreateMap<CandidateSkill, ReadedCandidateSkillContract>();
            CreateMap<CreateCandidateSkillContract, CandidateSkill>();
            CreateMap<CandidateSkill, CreatedCandidateSkillContract>();
            CreateMap<UpdateCandidateSkillContract, CandidateSkill>();
        }
    }
}
