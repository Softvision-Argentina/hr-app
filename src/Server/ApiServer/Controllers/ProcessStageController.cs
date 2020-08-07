// <copyright file="ProcessStageController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class ProcessStageController : BaseController<ProcessStageController>
    {
        private readonly IProcessStageService processStageService;
        private readonly IMapper mapper;

        public ProcessStageController(
            IProcessStageService processStageService,
            ILog<ProcessStageController> logger,
            IMapper mapper)
            : base(logger)
        {
            this.processStageService = processStageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var stages = this.processStageService.List();

                return this.Accepted(this.mapper.Map<List<ReadedStageViewModel>>(stages));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var stage = this.processStageService.Read(id);

                return this.Accepted(this.mapper.Map<ReadedStageViewModel>(stage));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateStageViewModel createStageVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateStageContract>(createStageVm);
                var returnContract = this.processStageService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedStageViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateStageViewModel updateStageVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateStageContract>(updateStageVm);
                contract.Id = id;

                this.processStageService.Update(contract);

                return this.Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.processStageService.Delete(id);

                return this.Accepted();
            });
        }
    }
}
