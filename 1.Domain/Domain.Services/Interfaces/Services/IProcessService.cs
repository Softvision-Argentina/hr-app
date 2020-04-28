using Domain.Services.Contracts.Process;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IProcessService
    {
        CreatedProcessContract Create(CreateProcessContract contract);
        ReadedProcessContract Read(int id);
        void Update(UpdateProcessContract contract);
        void Delete(int id);
        IEnumerable<ReadedProcessContract> List();
        IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId);
        IEnumerable<ReadedProcessContract> GetProcessesByCommunity(string community);
        void Approve(int processID);
        void Reject(int id, string rejectionReason);
    }
}
