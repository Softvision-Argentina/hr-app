using Microsoft.AspNetCore.Http;

namespace Domain.Services.Contracts.Cv
{
    public class CvContractAdd
    {
        public string UrlId { get; set; }
        public int CandidateId { get; set; }
        public string PublicId { get; set; }
        public IFormFile File { get; set; }
    }
}
