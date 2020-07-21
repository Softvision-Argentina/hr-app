using ApiServer.Contracts.Community;
using System.Collections.Generic;

namespace ApiServer.Contracts.CandidateProfile
{
    public class CreateCandidateProfileViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CreateCommunityViewModel> CommunityItems { get; set; }
    }
}
