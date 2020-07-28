using AutoMapper;
using Domain.Services.Contracts.Cv;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]/{candidateId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvService _cvService;
        private readonly ICandidateService _candidateService;
        private readonly IAzureUploadService _cvUploadService;

        public CvController(
            ICandidateService candidateService,
            IAzureUploadService cvUploadService,
            ICvService cvService)
        {
            _cvService = cvService;
            _candidateService = candidateService;
            _cvUploadService = cvUploadService;
        }

        [HttpPost]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> AddCv(int candidateId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = _candidateService.GetCandidate(candidateId);
            var file = cvContract.File;

            var filename = await _cvUploadService.Upload(file, candidate);

             _cvService.StoreCvAndCandidateCvId(candidate, cvContract, filename);

            return Ok("FileUploaded");
        }
    }
}
