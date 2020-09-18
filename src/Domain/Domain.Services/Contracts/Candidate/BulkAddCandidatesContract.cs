namespace Domain.Services.Contracts.Candidate
{
    using Domain.Services.Contracts.Community;
    using Microsoft.AspNetCore.Http;

    public class BulkAddCandidatesContract
    {
        public int CommunityId { get; set; }

        public string Source { get; set; }

        public IFormFile File { get; set; }
    }
}
