using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Skills;

namespace ApiServer.Contracts.CandidateSkill
{
    public class CreateCandidateSkillViewModel
    {
        public int CandidateId { get; set; }
        public CreateCandidateViewModel Candidate { get; set; }
        public int SkillId { get; set; }
        public CreateSkillViewModel Skill { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}
