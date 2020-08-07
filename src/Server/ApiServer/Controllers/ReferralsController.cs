// <copyright file="ReferralsController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using ApiServer.Contracts.Candidates;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ReferralsController : BaseController<ReferralsController>
    {
        private readonly ICandidateService candidateService;
        private readonly IMapper mapper;

        public ReferralsController(
            ICandidateService candidateService,
            ILog<ReferralsController> logger,
            IMapper mapper) : base(logger)
        {
            this.candidateService = candidateService;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCandidateViewModel createCandidateVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateCandidateContract>(createCandidateVm);

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
    }
}
