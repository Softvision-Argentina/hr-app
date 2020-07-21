using Domain.Services.Contracts.CandidateProfile;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ICandidateProfileService
    {
        CreatedCandidateProfileContract Create(CreateCandidateProfileContract contract);
        ReadedCandidateProfileContract Read(int id);
        void Update(UpdateCandidateProfileContract contract);
        void Delete(int id);
        IEnumerable<ReadedCandidateProfileContract> List();
    }
}
