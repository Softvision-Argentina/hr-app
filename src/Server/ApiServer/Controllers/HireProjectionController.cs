// <copyright file="HireProjectionController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.HireProjection;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.HireProjection;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class HireProjectionsController : BaseController<HireProjectionsController>
    {
        private readonly IHireProjectionService hireProjectionService;
        private readonly IMapper mapper;

        public HireProjectionsController(
            IHireProjectionService hireProjectionService,
            ILog<HireProjectionsController> logger,
            IMapper mapper) : base(logger)
        {
            this.hireProjectionService = hireProjectionService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var hireProjections = this.hireProjectionService.List();

                return this.Accepted(this.mapper.Map<List<ReadedHireProjectionViewModel>>(hireProjections));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var hireProjection = this.hireProjectionService.Read(id);

                if (hireProjection == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedHireProjectionViewModel>(hireProjection));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateHireProjectionViewModel createHireProjectVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateHireProjectionContract>(createHireProjectVm);
                var returnContract = this.hireProjectionService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedHireProjectionViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateHireProjectionViewModel updateHireProjectionVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateHireProjectionContract>(updateHireProjectionVm);
                contract.Id = id;
                this.hireProjectionService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.hireProjectionService.Delete(id);
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