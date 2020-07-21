using Core;

namespace Domain.Model
{
    public class Cv : Entity<int>
    {
        public string UrlId { get; set; }
        public Candidate Candidate { get; set; }
        public int CandidateId { get; set; }
        public string PublicId { get; set; }
    }
}
