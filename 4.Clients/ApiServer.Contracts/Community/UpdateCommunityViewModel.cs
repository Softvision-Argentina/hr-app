using ApiServer.Contracts.TaskItem;
using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.CandidateProfile;

namespace ApiServer.Contracts.Community
{
    public class UpdateCommunityViewModel
    {
        public string name { get; set; }

        public string Description { get; set; }
        public int ProfileId { get; set; }
        public UpdateCandidateProfileViewModel Profile { get; set; }
    }
}
