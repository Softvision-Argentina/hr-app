namespace Domain.Services.Contracts.SkillProfile
{
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Contracts.SkillType;

    public class UpdateSkillProfileViewModel
    {
        public int SkillId { get; set; }

        public ReadedSkillTypeViewModel Skill { get; set; }

        public int ProfileId { get; set; }

        public ReadedCandidateProfileViewModel Profile { get; set; }
    }
}
