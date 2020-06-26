using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Cv;
using Domain.Services.Interfaces.Repositories;
using Domain.Services.Interfaces.Services;

namespace Domain.Services.Impl.Services
{
    public class CvService : ICvService
    {
        private readonly ICvRepository _cvRepo;
        private readonly IMapper _mapper;
        public CvService(ICvRepository cvRepo, IMapper mapper)
        {
            _cvRepo = cvRepo;
            _mapper = mapper;
        }

        public void StoreCvAndCandidateCvId(Candidate candidate, CvContractAdd cvContract, Google.Apis.Drive.v3.Data.File fileUploaded)
        {
            cvContract.UrlId = fileUploaded.WebViewLink.Replace("drivesdk","sharing");
            cvContract.CandidateId = candidate.Id;
            candidate.Cv = cvContract.UrlId;

            var cv = _mapper.Map<Cv>(cvContract);
            _mapper.Map<Candidate>(candidate);

            _cvRepo.SaveAll(cv);
        }
    }
}
