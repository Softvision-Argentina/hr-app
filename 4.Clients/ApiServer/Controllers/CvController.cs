using AutoMapper;
using Domain.Services.Contracts.Cv;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.Interfaces.Services;
using Domain.Services.Interfaces.Repositories;
using System;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Domain.Model;

namespace ApiServer.Controllers
{
    [Route("api/[controller]/{candidateId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvRepository _repo;
        private readonly IMapper _mapper;
        ICandidateService _candidateService;
        ICvUploadService _cv;

        public CvController(ICvRepository repo, ICandidateService candidateService, IMapper mapper, ICvUploadService cv)
        {
            _repo = repo;
            _mapper = mapper;
            _candidateService = candidateService;
            _cv = cv;
        }

        [HttpPost]
        public IActionResult AddCv(int candidateId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = _candidateService.GetCandidate(candidateId);
            var file = cvContract.File;

            var auth = _cv.Authorize();
            var fileUploaded = _cv.Upload(auth, file);

            cvContract.UrlId = fileUploaded.Id;
            cvContract.CandidateId = candidate.Id;
            candidate.Cv = cvContract.UrlId;

            var cv = _mapper.Map<Cv>(cvContract);
             _mapper.Map<Candidate>(candidate);

            _repo.SaveAll(cv);

            return Ok("FileUploaded");
        }
    }
}
