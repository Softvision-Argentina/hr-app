namespace Domain.Services.Interfaces.Services
{
    using Domain.Model;
    using Domain.Services.Contracts.Cv;

    public interface ICvService
    {
        void StoreCvAndCandidateCvId(Candidate candidate, CvContractAdd cvToAdd, string filename);
    }
}