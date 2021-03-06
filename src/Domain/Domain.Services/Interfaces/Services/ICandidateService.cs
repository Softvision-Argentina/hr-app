﻿// <copyright file="ICandidateService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System;
    using System.Collections.Generic;
    using Domain.Model;
    using Domain.Services.Contracts.Candidate;
    using Microsoft.AspNetCore.Http;

    public interface ICandidateService
    {
        CreatedCandidateContract Create(CreateCandidateContract contract);

        ReadedCandidateContract Read(int id);

        ReadedCandidateContract Exists(int id);

        bool Exists(string email, int id);

        void Update(UpdateCandidateContract contract);

        void Delete(int id);

        void Reactivate(int id);

        IEnumerable<ReadedCandidateContract> List();

        IEnumerable<ReadedCandidateAppContract> ListApp();

        IEnumerable<ReadedCandidateContract> Read(Func<Candidate, bool> filter);

        Candidate GetCandidate(int id);

        IEnumerable<CandidateForReferralsContract> GetCandidatesForReferralComponent(int id);

        public void BulkCreate(IFormFile file, int communityId, string source);
    }
}
