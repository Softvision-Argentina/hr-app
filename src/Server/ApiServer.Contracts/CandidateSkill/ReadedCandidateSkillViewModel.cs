using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Skills;

namespace ApiServer.Contracts.CandidateSkill
{
    public class ReadedCandidateSkillViewModel
    {
        public int CandidateId { get; set; }
        public ReadedCandidateViewModel Candidate { get; set; }
        public int SkillId { get; set; }
        public ReadedSkillViewModel Skill { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}
