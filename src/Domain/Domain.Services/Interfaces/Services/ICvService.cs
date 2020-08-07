// <copyright file="ICvService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using Domain.Model;
    using Domain.Services.Contracts.Cv;

    public interface ICvService
    {
        void StoreCvAndCandidateCvId(Candidate candidate, CvContractAdd cvToAdd, Google.Apis.Drive.v3.Data.File filename);
    }
}
