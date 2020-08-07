namespace Domain.Services.Impl.Services
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Cv;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;

    public class CvService : ICvService
    {
        private readonly ICvRepository cvRepo;

        private readonly IMapper mapper;

        public CvService(ICvRepository cvRepo, IMapper mapper)
        {
            this.cvRepo = cvRepo;
            this.mapper = mapper;
        }

        public void StoreCvAndCandidateCvId(Candidate candidate, CvContractAdd cvContract, Google.Apis.Drive.v3.Data.File fileUploaded)
        {
            cvContract.UrlId = fileUploaded.WebViewLink.Replace("drivesdk", "sharing");
            cvContract.CandidateId = candidate.Id;
            candidate.Cv = cvContract.UrlId;

            var cv = this.mapper.Map<Cv>(cvContract);
            this.mapper.Map<Candidate>(candidate);

            this.cvRepo.SaveAll(cv);
        }
    }
}
