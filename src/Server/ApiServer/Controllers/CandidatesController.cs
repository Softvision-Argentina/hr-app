// <copyright file="CandidatesController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApiServer.Contracts.Candidates;
    using AutoMapper;
    using Core;
    using Domain.Model;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : BaseController<CandidatesController>
    {
        private readonly ICandidateService candidateService;
        private readonly IMapper mapper;

        public CandidatesController(
            ICandidateService candidateService,
            ILog<CandidatesController> logger,
            IMapper mapper) : base(logger)
        {
            this.candidateService = candidateService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var candidates = this.candidateService.List();

                return this.Accepted(this.mapper.Map<List<ReadedCandidateViewModel>>(candidates));
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

            return this.ApiAction(() =>
            {
                var candidates = this.candidateService.Read(filter);
                return this.Accepted(this.mapper.Map<List<ReadedCandidateViewModel>>(candidates));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var candidate = this.candidateService.Read(id);

                if (candidate == null)
                {
                    return this.NotFound(id);
                }

                var vm = this.mapper.Map<ReadedCandidateViewModel>(candidate);
                return this.Accepted(vm);
            });
        }

        [HttpGet("Exists/{id}")]
        public IActionResult Exists(int id)
        {
            return this.ApiAction(() =>
            {
                var candidate = this.candidateService.Exists(id);

                if (candidate == null)
                {
                    return this.Accepted();
                }

                var readedCandidateVm = this.mapper.Map<ReadedCandidateViewModel>(candidate);
                return this.Accepted(readedCandidateVm);
            });
        }

        [HttpGet("EmailExists/{email}/{id}")]
        public IActionResult EmailExists(string email, int id)
        {
            return this.ApiAction(() =>
            {
                var candidate = this.candidateService.Exists(email, id);

                if (!candidate)
                {
                    return this.Ok(new { Exists = false });
                }

                return this.Ok(new { Exists = true });
            });
        }

        [HttpGet("GetApp")]
        public IActionResult GetCandidateApp()
        {
            return this.ApiAction(() =>
            {
                var candidates = this.candidateService.ListApp();

                return this.Accepted(this.mapper.Map<List<ReadedCandidateAppViewModel>>(candidates));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCandidateViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateCandidateContract>(vm);
                var returnContract = this.candidateService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedCandidateViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCandidateViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateCandidateContract>(vm);
                contract.Id = id;
                this.candidateService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.candidateService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }

        [HttpPost("BulkAdd")]
        public IActionResult BulkAdd([FromForm] BulkAddCandidatesContract contract)
        {
            this.candidateService.BulkCreate(contract.File, contract.CommunityId, contract.Source);
            return this.Accepted();
        }
    }
}