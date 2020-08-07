// <copyright file="OpenPositionController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.OpenPosition;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.OpenPositions;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class OpenPositionController : BaseController<OpenPositionController>
    {
        private readonly IOpenPositionService openPositionService;
        private readonly IMapper mapper;

        public OpenPositionController(
            IOpenPositionService openPositionService,
            ILog<OpenPositionController> logger,
            IMapper mapper) : base(logger)
        {
            this.openPositionService = openPositionService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var openPositions = this.openPositionService.Get();

                return this.Accepted(this.mapper.Map<List<ReadedOpenPositionViewModel>>(openPositions));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var position = this.openPositionService.GetById(id);

                if (position == null)
                {
                    return this.NotFound(id);
                }

                var vm = this.mapper.Map<ReadedOpenPositionViewModel>(position);

                return this.Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOpenPositionViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateOpenPositionContract>(vm);
                var returnContract = this.openPositionService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedOpenPositionViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateOpenPositionViewModel vm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateOpenPositionContract>(vm);
                contract.Id = id;
                this.openPositionService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.openPositionService.Delete(id);
                return this.Accepted();
            });
        }
    }
}