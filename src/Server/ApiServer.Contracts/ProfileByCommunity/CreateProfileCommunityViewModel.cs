namespace ApiServer.Contracts.ProfileByCommunity
{
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Contracts.Community;

    public class CreateProfileCommunityViewModel
    {
        public ReadedCommunityViewModel Community { get; set; }

        public ReadedCandidateProfileViewModel Profile { get; set; }
    }
}
