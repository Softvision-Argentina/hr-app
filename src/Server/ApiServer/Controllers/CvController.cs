namespace ApiServer.Controllers
{
    using AutoMapper;
    using Domain.Services.Contracts.Cv;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/{candidateId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvService cvService;
        private readonly ICandidateService candidateService;
        private readonly IGoogleDriveUploadService cvUploadService;

        public CvController(
            ICandidateService candidateService,
            IGoogleDriveUploadService cvUploadService,
            ICvService cvService)
        {
            this.cvService = cvService;
            this.candidateService = candidateService;
            this.cvUploadService = cvUploadService;
        }

        [HttpPost]
        public IActionResult AddCv(int candidateId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = this.candidateService.GetCandidate(candidateId);

            var file = cvContract.File;
            var auth = this.cvUploadService.Authorize();

            var fileUploaded = this.cvUploadService.Upload(auth, file);

            this.cvService.StoreCvAndCandidateCvId(candidate, cvContract, fileUploaded);

            return this.Ok("FileUploaded");
        }
    }
}