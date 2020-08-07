// <copyright file="DaysOffController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ApiServer.Contracts.DaysOff;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.DaysOff;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class DaysOffController : BaseController<DaysOffController>
    {
        private readonly IDaysOffService daysOffService;
        private readonly IMapper mapper;

        public DaysOffController(
            IDaysOffService daysOffService,
            ILog<DaysOffController> logger,
            IMapper mapper) : base(logger)
        {
            this.daysOffService = daysOffService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var daysOff = this.daysOffService.List();

                return this.Accepted(this.mapper.Map<List<ReadedDaysOffViewModel>>(daysOff));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var daysOff = this.daysOffService.Read(id);

                if (daysOff == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedDaysOffViewModel>(daysOff));
            });
        }

        [HttpGet("GetByDni")]
        public IActionResult GetByDni([FromQuery] int dni)
        {
            return this.ApiAction(() =>
            {
                var daysOff = this.daysOffService.ReadByDni(dni);

                if ((daysOff == null) || (daysOff.Count() == 0))
                {
                    return this.NotFound(dni);
                }

                return this.Accepted(this.mapper.Map<List<ReadedDaysOffViewModel>>(daysOff));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDaysOffViewModel createDaysOffVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateDaysOffContract>(createDaysOffVm);
                var returnContract = this.daysOffService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedDaysOffViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateDaysOffViewModel updateDaysOffVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateDaysOffContract>(updateDaysOffVm);
                contract.Id = id;
                this.daysOffService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.daysOffService.Delete(id);
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