﻿using System.Collections.Generic;
using ApiServer.Contracts.Dashboard;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public DashboardController(
            IDashboardService dashboardService,
            ILog<DashboardController> logger,
            IMapper mapper) : base(logger)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var dashboards = _dashboardService.List();

                return Accepted(_mapper.Map<List<ReadedDashboardViewModel>>(dashboards));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var dashboard = _dashboardService.Read(id);

                if (dashboard == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedDashboardViewModel>(dashboard));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateDashboardViewModel createDashboardVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDashboardContract>(createDashboardVm);
                var returnContract = _dashboardService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDashboardViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateDashboardViewModel updateDashboardVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDashboardContract>(updateDashboardVm);
                contract.Id = id;
                _dashboardService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _dashboardService.Delete(id);
                return Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}