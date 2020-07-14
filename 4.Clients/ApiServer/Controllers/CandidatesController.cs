using System.Collections.Generic;
using ApiServer.Contracts.Candidates;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Domain.Model;

namespace ApiServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : BaseController<CandidatesController>
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public CandidatesController(
            ICandidateService candidateService,
            ILog<CandidatesController> logger,
            IMapper mapper) : base(logger)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var candidates = _candidateService.List();

                return Accepted(_mapper.Map<List<ReadedCandidateViewModel>>(candidates));
            });
        }

        [HttpPost("filter")]
        public IActionResult Get([FromBody] FilterCandidateViewModel filterData)
        {

            Func<Candidate, bool> filterByPrefferedOffice = candidate => filterData.PreferredOffice == null ? true : candidate.PreferredOffice.Id.Equals(filterData.PreferredOffice);
            Func<Candidate, bool> filterByCommunity = candidate => filterData.Community == null ? true : candidate.Community.Id.Equals(filterData.Community);


            Func<Candidate, bool> filter = candidate => filterByPrefferedOffice(candidate)
            && filterByCommunity(candidate)
            && filterData.SelectedSkills
            .All(requiredSkill =>
            candidate.CandidateSkills
            .Where(skill => skill.Rate >= requiredSkill.MinRate && skill.Rate <= requiredSkill.MaxRate)
            .Select(skill => skill.SkillId)
            .Contains(requiredSkill.SkillId));

            return ApiAction(() =>
            {
                var candidates = _candidateService.Read(filter);
                return Accepted(_mapper.Map<List<ReadedCandidateViewModel>>(candidates));

            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var candidate = _candidateService.Read(id);

                if (candidate == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<ReadedCandidateViewModel>(candidate);
                return Accepted(vm);
            });
        }

        [HttpGet("Exists/{id}")]
        public IActionResult Exists(int id)
        {
            return ApiAction(() =>
            {
                var candidate = _candidateService.Exists(id);

                if (candidate == null)
                {
                    return Accepted();
                }

                var readedCandidateVm = _mapper.Map<ReadedCandidateViewModel>(candidate);
                return Accepted(readedCandidateVm);
            });
        }

        [HttpGet("EmailExists/{email}")]
        public IActionResult EmailExists(string email)
        {
            return ApiAction(() =>
            {
                if (_candidateService.Exists(email))
                {
                    return Ok(new { Exists = true
                    });
                }

                return Ok(new { Exists = false } );
            });
        }

        [HttpGet("GetApp")]
        public IActionResult GetCandidateApp()
        {
            return ApiAction(() =>
            {
                var candidates = _candidateService.ListApp();

                return Accepted(_mapper.Map<List<ReadedCandidateAppViewModel>>(candidates));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateCandidateViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCandidateContract>(vm);
                var returnContract = _candidateService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCandidateViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateCandidateViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateCandidateContract>(vm);
                contract.Id = id;
                _candidateService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _candidateService.Delete(id);
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