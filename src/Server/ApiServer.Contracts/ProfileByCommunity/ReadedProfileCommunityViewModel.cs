namespace ApiServer.Contracts.ProfileByCommunity
{
    using ApiServer.Contracts.CandidateProfile;

    public class ReadedProfileCommunityViewModel
    {
        public int Id { get; set; }

        public ReadedCandidateProfileViewModel Profile { get; set; }
    }
}
