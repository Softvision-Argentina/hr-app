// <copyright file="SkillTypesController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.SkillType;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.SkillType;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class SkillTypesController : BaseController<SkillTypesController>
    {
        private readonly ISkillTypeService skillTypeService;
        private readonly IMapper mapper;

        public SkillTypesController(
            ISkillTypeService skillTypeService,
            ILog<SkillTypesController> logger,
            IMapper mapper) : base(logger)
        {
            this.skillTypeService = skillTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var skillsTypes = this.skillTypeService.List();

                return this.Accepted(this.mapper.Map<List<ReadedSkillTypeViewModel>>(skillsTypes));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var skillTpe = this.skillTypeService.Read(id);

                if (skillTpe == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedSkillTypeViewModel>(skillTpe));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateSkillTypeViewModel createSkillTypeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateSkillTypeContract>(createSkillTypeVm);
                var returnContract = this.skillTypeService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedSkillTypeViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateSkillTypeViewModel updateSkillTypeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateSkillTypeContract>(updateSkillTypeVm);
                contract.Id = id;
                this.skillTypeService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.skillTypeService.Delete(id);
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