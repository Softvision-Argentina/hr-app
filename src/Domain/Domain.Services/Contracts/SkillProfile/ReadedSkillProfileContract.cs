namespace Domain.Services.Contracts.SkillProfile
{
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Contracts.SkillType;

    public class ReadedSkillProfileContract
    {
        public int SkillId { get; set; }

        public ReadedSkillTypeContract Skill { get; set; }

        public int ProfileId { get; set; }

        public ReadedCandidateProfileContract Profile { get; set; }
    }
}
