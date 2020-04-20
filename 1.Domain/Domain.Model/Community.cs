using Core;

namespace Domain.Model
{
    public class Community : DescriptiveEntity<int>
    {
        public int ProfileId { get; set; }
        public CandidateProfile Profile { get; set; }
    }
}
