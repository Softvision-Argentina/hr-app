// <copyright file="CvContractAdd.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Cv
{
    using Microsoft.AspNetCore.Http;

    public class CvContractAdd
    {
        public string UrlId { get; set; }

        public int CandidateId { get; set; }

        public string PublicId { get; set; }

        public IFormFile File { get; set; }
    }
}
