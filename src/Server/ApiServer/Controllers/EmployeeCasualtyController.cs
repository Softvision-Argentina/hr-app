// <copyright file="EmployeeCasualtyController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.EmployeeCasualty;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.EmployeeCasualty;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCasualtyController : BaseController<EmployeeCasualtyController>
    {
        private readonly IEmployeeCasualtyService employeeCasualtyService;
        private readonly IMapper mapper;

        public EmployeeCasualtyController(
            IEmployeeCasualtyService employeeCasualtyService,
            ILog<EmployeeCasualtyController> logger,
            IMapper mapper) : base(logger)
        {
            this.employeeCasualtyService = employeeCasualtyService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var employeeCasualty = this.employeeCasualtyService.List();

                return this.Accepted(this.mapper.Map<List<ReadedEmployeeCasualtyViewModel>>(employeeCasualty));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var employeeCasualty = this.employeeCasualtyService.Read(id);

                if (employeeCasualty == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedEmployeeCasualtyViewModel>(employeeCasualty));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateEmployeeCasualtyViewModel createEmployeeCasualtyVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateEmployeeCasualtyContract>(createEmployeeCasualtyVm);
                var returnContract = this.employeeCasualtyService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedEmployeeCasualtyViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateEmployeeCasualtyViewModel updateEmployeeCasualtyVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateEmployeeCasualtyContract>(updateEmployeeCasualtyVm);
                contract.Id = id;
                this.employeeCasualtyService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.employeeCasualtyService.Delete(id);
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