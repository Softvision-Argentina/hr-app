using Domain.Services.Contracts.CandidateProfile;

namespace Domain.Services.Contracts.Community
{
    public class CreateCommunityContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProfileId { get; set; }
        public CreateCandidateProfileContract Profile { get; set; }
    }
}
