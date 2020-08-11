namespace ApiServer.Controllers
{
    using Domain.Services.Contracts.Cv;
    using Microsoft.AspNetCore.Mvc;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Cors;
    using System.Threading.Tasks;

    [Route("api/[controller]/{candidateId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvService cvService;
        private readonly ICandidateService candidateService;
        private readonly IAzureUploadService cvUploadService;

        public CvController(
            ICandidateService candidateService,
            IAzureUploadService cvUploadService,
            ICvService cvService)
        {
            this.cvService = cvService;
            this.candidateService = candidateService;
            this.cvUploadService = cvUploadService;
        }

        [HttpPost]
        [EnableCors("AllowAll")]
        public async Task<IActionResult> AddCv(int candidateId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = this.candidateService.GetCandidate(candidateId);
            var file = cvContract.File;

            var filename = await this.cvUploadService.Upload(file, candidate);

            this.cvService.StoreCvAndCandidateCvId(candidate, cvContract, filename);

            return this.Ok("FileUploaded");
        }
    }
}
