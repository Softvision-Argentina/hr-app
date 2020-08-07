// <copyright file="SkillsController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Skills;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Skill;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : BaseController<SkillsController>
    {
        private readonly ISkillService skillService;
        private readonly IMapper mapper;

        public SkillsController(
            ISkillService skillService,
            ILog<SkillsController> logger,
            IMapper mapper) : base(logger)
        {
            this.skillService = skillService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var skills = this.skillService.List();

                return this.Accepted(this.mapper.Map<List<ReadedSkillViewModel>>(skills));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var skill = this.skillService.Read(id);

                if (skill == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedSkillViewModel>(skill));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateSkillViewModel createSkillsVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateSkillContract>(createSkillsVm);
                var returnContract = this.skillService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedSkillViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateSkillViewModel updateSkillsVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateSkillContract>(updateSkillsVm);
                contract.Id = id;
                this.skillService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.skillService.Delete(id);
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