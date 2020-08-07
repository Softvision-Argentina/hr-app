// <copyright file="DashboardController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Dashboard;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService dashboardService;
        private readonly IMapper mapper;

        public DashboardController(
            IDashboardService dashboardService,
            ILog<DashboardController> logger,
            IMapper mapper) : base(logger)
        {
            this.dashboardService = dashboardService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var dashboards = this.dashboardService.List();

                return this.Accepted(this.mapper.Map<List<ReadedDashboardViewModel>>(dashboards));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var dashboard = this.dashboardService.Read(id);

                if (dashboard == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedDashboardViewModel>(dashboard));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDashboardViewModel createDashboardVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateDashboardContract>(createDashboardVm);
                var returnContract = this.dashboardService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedDashboardViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateDashboardViewModel updateDashboardVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateDashboardContract>(updateDashboardVm);
                contract.Id = id;
                this.dashboardService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.dashboardService.Delete(id);
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