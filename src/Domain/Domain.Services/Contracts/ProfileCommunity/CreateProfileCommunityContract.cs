namespace Domain.Services.Contracts.ProfileByCommunity
{
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Contracts.Community;

    public class CreateProfileCommunityContract
    {
        public ReadedCommunityContract Community { get; set; }

        public ReadedCandidateProfileContract Profile { get; set; }
    }
}
