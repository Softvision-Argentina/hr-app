using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.Cv
{
    public class CvContractAdd
    {
        public string Url { get; set; }

        public int CandidateId { get; set; }

        public string PublicId { get; set; }

        public IFormFile File { get; set; }
    }
}
