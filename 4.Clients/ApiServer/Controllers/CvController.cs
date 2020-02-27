using ApiServer.Helpers;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Model;
using Domain.Services.Contracts.Cv;
using Domain.Services.Repositories.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
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
        private readonly IOptions<CloudinarySettings> _cloud;
        ICandidateService _candidateService;
        private readonly Cloudinary _cloudinary;

        public CvController(ICvRepository repo, ICandidateService candidateService, IMapper mapper, IOptions<CloudinarySettings> cloud)
        {
            _repo = repo;
            _mapper = mapper;
            _cloud = cloud;
            _candidateService = candidateService;

            Account acc = new Account("da9gsxocz", "355671172844856", "VWdCXBbICFFANmGDo0hgGWmi2hU");

            _cloudinary = new Cloudinary(acc);
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

            var candidate = _candidateService.GetCandidate(candId);

            var file = cvContract.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            cvContract.Url = uploadResult.Uri.ToString();
            cvContract.PublicId = uploadResult.PublicId;
            cvContract.CandidateId = candidate.Id;
            candidate.Cv = cvContract.Url;

            var cv = _mapper.Map<Cv>(cvContract);
            var cand = _mapper.Map<Candidate>(candidate);

            if(_repo.SaveAll(cv))
            {
                var cvReturn = _mapper.Map<CvContractReturn>(cv);
                return CreatedAtRoute("GetCv", new { candId, id = cv.Id }, cvReturn);
            }

            return BadRequest("Could not load the cv");
        }
    }
}
