// <copyright file="IProcessService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Model;
    using Domain.Services.Contracts.Process;

    public interface IProcessService
    {
        Process Create(CreateProcessContract contract);

        ReadedProcessContract Read(int id);

        Process Update(UpdateProcessContract contract);

        void Delete(int id);

        IEnumerable<ReadedProcessContract> List();

        IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId);

        IEnumerable<ReadedProcessContract> GetProcessesByCommunity(string community);

        Process Approve(int processID);

        IEnumerable<ReadedProcessContract> GetDeletedProcesses();

        ReadedProcessContract Reactivate(int processID);

        void Reject(int id, string rejectionReason);
    }
}
