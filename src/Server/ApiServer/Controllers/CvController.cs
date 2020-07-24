using AutoMapper;
using Domain.Services.Contracts.Cv;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Cors;

namespace ApiServer.Controllers
{
    [Route("api/[controller]/{candidateId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvService _cvService;
        private readonly ICandidateService _candidateService;
        private readonly IGoogleDriveUploadService _cvUploadService;

        public CvController(
            ICandidateService candidateService,
            IGoogleDriveUploadService cvUploadService,
            ICvService cvService)
        {
            _cvService = cvService;
            _candidateService = candidateService;
            _cvUploadService = cvUploadService;
        }

        [EnableCors()]
        [HttpPost]
        public IActionResult AddCv(int candidateId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = _candidateService.GetCandidate(candidateId);
            
            var file = cvContract.File;
            var auth = _cvUploadService.Authorize();
            
            var fileUploaded = _cvUploadService.Upload(auth, file);

             _cvService.StoreCvAndCandidateCvId(candidate, cvContract, fileUploaded);

            return Ok("FileUploaded");
        }
    }
}
