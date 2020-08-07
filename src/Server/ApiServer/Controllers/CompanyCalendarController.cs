// <copyright file="CompanyCalendarController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.CompanyCalendar;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.CompanyCalendar;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyCalendarController : BaseController<CompanyCalendarController>
    {
        private readonly ICompanyCalendarService companyCalendarService;
        private readonly IMapper mapper;

        public CompanyCalendarController(
            ICompanyCalendarService companyCalendarService,
            ILog<CompanyCalendarController> logger,
            IMapper mapper) : base(logger)
        {
            this.companyCalendarService = companyCalendarService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var companyCalendar = this.companyCalendarService.List();

                return this.Accepted(this.mapper.Map<List<ReadedCompanyCalendarViewModel>>(companyCalendar));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var companyCalendar = this.companyCalendarService.Read(id);

                if (companyCalendar == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedCompanyCalendarViewModel>(companyCalendar));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCompanyCalendarViewModel createCompanyCalendarVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateCompanyCalendarContract>(createCompanyCalendarVm);
                var returnContract = this.companyCalendarService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedCompanyCalendarViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCompanyCalendarViewModel updateCompanyCalendarVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateCompanyCalendarContract>(updateCompanyCalendarVm);
                contract.Id = id;
                this.companyCalendarService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.companyCalendarService.Delete(id);
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