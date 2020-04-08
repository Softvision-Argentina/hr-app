using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public class CandidateProfileController : BaseController<CandidateProfileController> {

        ICandidateProfileService _CandidateProfileService;
        private IMapper _mapper;

        public CandidateProfileController(ICandidateProfileService CandidateProfileService,
                                   ILog<CandidateProfileController> logger,
                                   IMapper mapper): base(logger)
        {
            _CandidateProfileService = CandidateProfileService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var CandidateProfiles = _CandidateProfileService.List();

                return Accepted(_mapper.Map<List<ReadedCandidateProfileViewModel>>(CandidateProfiles));
            });
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            return ApiAction(() =>
            {
                var CandidateProfile = _CandidateProfileService.Read(Id);

                if (CandidateProfile == null)
                {
                    return NotFound(Id);
                }

                return Accepted(_mapper.Map<ReadedCandidateProfileViewModel>(CandidateProfile));
            });
        }

        // POST api/skills
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody] CreateCandidateProfileViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCandidateProfileContract>(vm);
                var returnContract = _CandidateProfileService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCandidateProfileViewModel>(returnContract));
            });
        }

        // PUT api/skills/5
        // Mutation
        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody]UpdateCandidateProfileViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateCandidateProfileContract>(vm);
                contract.Id = Id;
                _CandidateProfileService.Update(contract);

                return Accepted(new { Id });
            });
        }

        // DELETE api/skills/5
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            return ApiAction(() =>
            {
                _CandidateProfileService.Delete(Id);
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