// <copyright file="IProcessService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Process;

    public interface IProcessService
    {
        CreatedProcessContract Create(CreateProcessContract contract);

        ReadedProcessContract Read(int id);

        void Update(UpdateProcessContract contract);

        void Delete(int id);

        IEnumerable<ReadedProcessContract> List();

        IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId);

        IEnumerable<ReadedProcessContract> GetProcessesByCommunity(string community);

        IEnumerable<ReadedProcessContract> GetDeletedProcesses();

        void Approve(int processID);

        void Reactivate(int processID);

        void Reject(int id, string rejectionReason);
    }
}
