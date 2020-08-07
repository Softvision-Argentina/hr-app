// <copyright file="DeclineReasonController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class DeclineReasonController : BaseController<DeclineReasonController>
    {
        private readonly IDeclineReasonService declineReasonService;
        private readonly IMapper mapper;

        public DeclineReasonController(
            IDeclineReasonService declineReasonService,
            ILog<DeclineReasonController> logger,
            IMapper mapper) : base(logger)
        {
            this.declineReasonService = declineReasonService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var declineReasons = this.declineReasonService.List();

                return this.Accepted(this.mapper.Map<List<ReadedDeclineReasonViewModel>>(declineReasons));
            });
        }

        [HttpGet("Named")]
        public IActionResult GetNamed()
        {
            return this.ApiAction(() =>
            {
                var declineReasons = this.declineReasonService.ListNamed();

                return this.Accepted(this.mapper.Map<List<ReadedDeclineReasonViewModel>>(declineReasons));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var skillTpe = this.declineReasonService.Read(id);

                if (skillTpe == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedDeclineReasonViewModel>(skillTpe));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDeclineReasonViewModel createDeclineReasonVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateDeclineReasonContract>(createDeclineReasonVm);
                var returnContract = this.declineReasonService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedDeclineReasonViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateDeclineReasonViewModel updateDeclineReasonVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateDeclineReasonContract>(updateDeclineReasonVm);
                contract.Id = id;
                this.declineReasonService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.declineReasonService.Delete(id);
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