using Domain.Model;
using Domain.Services.Contracts.Cv;

namespace Domain.Services.Interfaces.Services
{
    public interface ICvService
    {
        void StoreCvAndCandidateCvId(Candidate candidate, CvContractAdd cvToAdd, Google.Apis.Drive.v3.Data.File fileUploaded);
    }
}
