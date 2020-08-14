namespace Domain.Model
{
    using Core;

    public class ProfileCommunity : Entity<int>
    {
        public Community Community { get; set; }

        public CandidateProfile Profile { get; set; }
    }
}
