using Core;
using System.Collections.Generic;

namespace Domain.Model
{
    public class CandidateProfile : DescriptiveEntity<int>
    {
        public IList<Community> CommunityItems { get; set; }
    }
}
