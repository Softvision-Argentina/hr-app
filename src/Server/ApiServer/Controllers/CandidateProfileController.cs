// <copyright file="CandidateProfileController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.CandidateProfile;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CandidateProfileController : BaseController<CandidateProfileController>
    {
        private readonly ICandidateProfileService candidateProfileService;
        private readonly IMapper mapper;

        public CandidateProfileController(
            ICandidateProfileService candidateProfileService,
            ILog<CandidateProfileController> logger,
            IMapper mapper) : base(logger)
        {
            this.candidateProfileService = candidateProfileService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var candidateProfiles = this.candidateProfileService.List();

                return this.Accepted(this.mapper.Map<List<ReadedCandidateProfileViewModel>>(candidateProfiles));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var candidateProfile = this.candidateProfileService.Read(id);

                if (candidateProfile == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedCandidateProfileViewModel>(candidateProfile));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCandidateProfileViewModel createCandidateProfileVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateCandidateProfileContract>(createCandidateProfileVm);
                var returnContract = this.candidateProfileService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedCandidateProfileViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCandidateProfileViewModel updateCandidateProfileVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateCandidateProfileContract>(updateCandidateProfileVm);
                contract.Id = id;
                this.candidateProfileService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.candidateProfileService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }
    }
}