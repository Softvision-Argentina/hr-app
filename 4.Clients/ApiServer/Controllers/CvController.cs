using AutoMapper;
using Domain.Services.Contracts.Cv;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.Interfaces.Services;
using Domain.Services.Interfaces.Repositories;

namespace ApiServer.Controllers
{
    [Route("api/[controller]/{candId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvRepository _repo;
        private readonly IMapper _mapper;
        ICandidateService _candidateService;

        public CvController(ICvRepository repo, ICandidateService candidateService, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _candidateService = candidateService;
        }

        [HttpGet("id", Name = "GetCv")]
        public IActionResult Getphoto(int id)
        {
            var cv = _repo.GetCv(id);

            var cvReturn = _mapper.Map<CvContractReturn>(cv);

            return Ok(cvReturn);
        }

        [HttpPost]
        public IActionResult AddPhoto(int candId, [FromForm] CvContractAdd cvContract)
        {
            //var candidate = _candidateService.Read(candId);

            //var candidate = _candidateService.GetCandidate(candId);

            //var file = cvContract.File;

            //var uploadResult = new ImageUploadResult();

            //if (file.Length > 0)
            //{
            //    using (var stream = file.OpenReadStream())
            //    {
            //        var uploadParams = new ImageUploadParams()
            //        {
            //            File = new FileDescription(file.Name, stream)
            //        };

            //        uploadResult = _cloudinary.Upload(uploadParams);
            //    }
            //}

            //cvContract.Url = uploadResult.Uri.ToString();
            //cvContract.PublicId = uploadResult.PublicId;
            //cvContract.CandidateId = candidate.Id;
            //candidate.Cv = cvContract.Url;

            //var cv = _mapper.Map<Cv>(cvContract);
            //var cand = _mapper.Map<Candidate>(candidate);

            //if(_repo.SaveAll(cv))
            //{
            //    var cvReturn = _mapper.Map<CvContractReturn>(cv);
            //    return CreatedAtRoute("GetCv", new { candId, id = cv.Id }, cvReturn);
            //}

            return Ok("FileUploaded");
        }
    }
}
