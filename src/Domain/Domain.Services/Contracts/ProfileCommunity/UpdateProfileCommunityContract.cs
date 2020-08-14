namespace Domain.Services.Contracts.ProfileByCommunity
{
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Contracts.Community;

    public class UpdateProfileCommunityContract
    {
        public int Id { get; set; }

        public ReadedCommunityContract CommunityContract { get; set; }

        public ReadedCandidateProfileContract ProfileContract { get; set; }
    }
}
