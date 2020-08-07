// <copyright file="EmployeesController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Employee;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Employee;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<EmployeesController>
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeesController(
            IEmployeeService employeeService,
            ILog<EmployeesController> logger,
            IMapper mapper) : base(logger)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return this.ApiAction(() =>
            {
                var employees = this.employeeService.List();
                return this.Accepted(this.mapper.Map<List<ReadedEmployeeViewModel>>(employees));
            });
        }

        // TODO: we could simplify this base on convention over configuration same for the rest of the endpoints
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            return this.ApiAction(() =>
            {
                var employee = this.employeeService.GetById(id);
                return this.Accepted(this.mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpGet("GetByDni")]
        public IActionResult GetByDNI([FromQuery] int dni)
        {
            return this.ApiAction(() =>
            {
                var employee = this.employeeService.GetByDNI(dni);
                return this.Accepted(this.mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail([FromQuery] string email)
        {
            return this.ApiAction(() =>
            {
                var employee = this.employeeService.GetByEmail(email);
                return this.Accepted(this.mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.employeeService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] UpdateEmployeeViewModel updateEmployeeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateEmployeeContract>(updateEmployeeVm);
                this.employeeService.UpdateEmployee(contract);

                return this.Accepted();
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody] CreateEmployeeViewModel createEmployeeVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateEmployeeContract>(createEmployeeVm);
                var returnContract = this.employeeService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedEmployeeViewModel>(returnContract));
            });
        }
    }
}
