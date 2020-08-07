// <copyright file="ICandidateProfileService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.CandidateProfile;

    public interface ICandidateProfileService
    {
        CreatedCandidateProfileContract Create(CreateCandidateProfileContract contract);

        ReadedCandidateProfileContract Read(int id);

        void Update(UpdateCandidateProfileContract contract);

        void Delete(int id);

        IEnumerable<ReadedCandidateProfileContract> List();
    }
}
