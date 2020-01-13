﻿using Domain.Model;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Interfaces.Services
{
    public interface ICandidateService
    {
        CreatedCandidateContract Create(CreateCandidateContract contract);
        ReadedCandidateContract Read(int id);
        ReadedCandidateContract Exists(int id);
        void Update(UpdateCandidateContract contract);
        void Delete(int id);
        IEnumerable<ReadedCandidateContract> List();
        IEnumerable<ReadedCandidateAppContract> ListApp();
        IEnumerable<ReadedCandidateContract> Read(Func<Candidate, bool> filter);
    }
}
