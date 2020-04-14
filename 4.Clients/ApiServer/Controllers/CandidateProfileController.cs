using System.Collections.Generic;
using ApiServer.Contracts.CandidateProfile;
using AutoMapper;
using Core;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateProfileController : BaseController<CandidateProfileController>
    {

        private readonly ICandidateProfileService _candidateProfileService;
        private readonly IMapper _mapper;

        public CandidateProfileController(
            ICandidateProfileService candidateProfileService,
            ILog<CandidateProfileController> logger,
            IMapper mapper) : base(logger)
        {
            _candidateProfileService = candidateProfileService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var candidateProfiles = _candidateProfileService.List();

                return Accepted(_mapper.Map<List<ReadedCandidateProfileViewModel>>(candidateProfiles));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var candidateProfile = _candidateProfileService.Read(id);

                if (candidateProfile == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedCandidateProfileViewModel>(candidateProfile));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCandidateProfileViewModel createCandidateProfileVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCandidateProfileContract>(createCandidateProfileVm);
                var returnContract = _candidateProfileService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCandidateProfileViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateCandidateProfileViewModel updateCandidateProfileVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateCandidateProfileContract>(updateCandidateProfileVm);
                contract.Id = id;
                _candidateProfileService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _candidateProfileService.Delete(id);
                return Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}