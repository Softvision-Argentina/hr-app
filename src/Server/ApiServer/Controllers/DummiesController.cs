// <copyright file="DummiesController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Seed;
    using ApiServer.Security;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Seed;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    // TODO:do we use actually this controller ? or we can remove it.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DummiesController : BaseController<DummiesController>
    {
        private readonly IDummyService dummyService;
        private readonly IMapper mapper;

        public DummiesController(
            IDummyService dummyService,
            ILog<DummiesController> logger,
            IMapper mapper)
            : base(logger)
        {
            this.dummyService = dummyService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = SecurityClaims.CANLISTDUMMY)]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var dummies = this.dummyService.List();

                return this.Accepted(this.mapper.Map<List<ReadedDummyViewModel>>(dummies));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return this.ApiAction(() =>
            {
                var dummy = this.dummyService.Read(id);

                if (dummy == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedDummyViewModel>(dummy));
            });
        }

        // POST api/dummies
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody] CreateDummyViewModel createDummyVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateDummyContract>(createDummyVm);
                var returnContract = this.dummyService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedDummyViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UpdateDummyViewModel updateDummyVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateDummyContract>(updateDummyVm);
                contract.Id = id;
                this.dummyService.Update(contract);

                return this.Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return this.ApiAction(() =>
            {
                this.dummyService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new PingViewModel { Status = "OK" });
        }
    }
}
