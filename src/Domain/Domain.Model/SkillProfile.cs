namespace Domain.Model
{
    public class SkillProfile
    {
        public int SkillId { get; set; }

        public SkillType Skill { get; set; }

        public int ProfileId { get; set; }

        public CandidateProfile Profile { get; set; }
    }
}
