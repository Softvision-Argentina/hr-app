﻿using System.Collections.Generic;
using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.CandidateSkill;
using AutoMapper;
using Core;
using Domain.Model.Enum;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        ICandidateService _candidateService;
        private IMapper _mapper;

        public CandidatesController(ICandidateService candidateService, 
                                 ILog<CandidatesController> logger, 
                                 IMapper mapper): base(logger)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Policy = SecurityClaims.CAN_LIST_CANDIDATE)]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var candidates = _candidateService.List();

                return Accepted(_mapper.Map<List<ReadedCandidateViewModel>>(candidates));
            });
        }

        [HttpPost("filter")]
        //[Authorize(Policy = SecurityClaims.CAN_LIST_CANDIDATE)]
        public IActionResult Get([FromBody]Dictionary<string,IEnumerable<FilterCandidateSkillViewModel>> filterData)
        {
         
            Func<Candidate, bool> filter = candidate => filterData["skills"]
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

        // GET api/candidates/5
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

        //// GET api/candidates/exists/1
        //[HttpGet("Exists/{dni}")]
        //public IActionResult Exists(int dni)
        //{
        //    return ApiAction(() =>
        //    {
        //        var candidate = _candidateService.Exists(dni);

        //        if (candidate == null)
        //        {
        //            return Accepted();
        //        }

        //        var vm = _mapper.Map<ReadedCandidateViewModel>(candidate);
        //        return Accepted(vm);
        //    });
        //}

        // GET api/candidates/exists/1
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

                var vm = _mapper.Map<ReadedCandidateViewModel>(candidate);
                return Accepted(vm);
            });
        }


        [HttpGet("GetApp")]
        //[Authorize(Policy = SecurityClaims.CAN_LIST_CANDIDATE)]
        public IActionResult GetCandidateApp()
        {
            return ApiAction(() =>
            {
                var candidates = _candidateService.ListApp();

                return Accepted(_mapper.Map<List<ReadedCandidateAppViewModel>>(candidates));
            });
        }

        // POST api/candidates
        // Creation
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


        // PUT api/candidates/5
        // Mutation
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

        // DELETE api/candidates/5
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