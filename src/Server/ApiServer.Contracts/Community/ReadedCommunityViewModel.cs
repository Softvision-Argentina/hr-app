using ApiServer.Contracts.CandidateProfile;

namespace ApiServer.Contracts.Community
{
    public class ReadedCommunityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProfileId { get; set; }
        public ReadedCandidateProfileViewModel Profile { get; set; }
    }
}
