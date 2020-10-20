// <copyright file="InterviewController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Interview;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Interview;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : BaseController<InterviewsController>
    {
        private readonly IInterviewService interviewService;
        private readonly IMapper mapper;

        public InterviewsController(
            IInterviewService interviewService,
            ILog<InterviewsController> logger,
            IMapper mapper) : base(logger)
        {
            this.interviewService = interviewService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var interviews = this.interviewService.List();

                return this.Accepted(this.mapper.Map<List<ReadedInterviewViewModel>>(interviews));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var interview = this.interviewService.Read(id);

                if (interview == null)
                {
                    return this.NotFound(id);
                }

                var vm = this.mapper.Map<ReadedInterviewViewModel>(interview);
                return this.Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateInterviewViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateInterviewContract>(vm);
                var returnContract = this.interviewService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedInterviewViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateInterviewViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateInterviewContract>(vm);
                contract.Id = id;
                this.interviewService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.interviewService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpPut("Update/{clientStageId}")]
        public IActionResult Update(int clientStageId, [FromBody] List<CreateInterviewContract> contracts)
        {
            return this.ApiAction(() =>
            {
                this.interviewService.UpdateMany(clientStageId, contracts);
                return this.Accepted();
            });
        }
    }
}