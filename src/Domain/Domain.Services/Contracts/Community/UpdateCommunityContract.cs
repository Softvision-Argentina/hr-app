using Domain.Services.Contracts.CandidateProfile;

namespace Domain.Services.Contracts.Community
{
    public class UpdateCommunityContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProfileId { get; set; }
        public UpdateCandidateProfileContract Profile { get; set; }

    }
}
