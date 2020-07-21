using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Skill;

namespace Domain.Services.Contracts.CandidateSkill
{
    public class UpdateCandidateSkillContract
    {
        public int CandidateId { get; set; }
        public ReadedCandidateContract Candidate { get; set; }
        public int SkillId { get; set; }
        public ReadedSkillContract Skill { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}
