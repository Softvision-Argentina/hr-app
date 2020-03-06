using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Contracts.Dashboard;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController<DashboardController>
    {
        IDashboardService _DashboardService;
        private IMapper _mapper;

        public DashboardController(IDashboardService DashboardService,
                                   ILog<DashboardController> logger,
                                   IMapper mapper) : base(logger)
        {
            _DashboardService = DashboardService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var Dashboards = _DashboardService.List();

                return Accepted(_mapper.Map<List<ReadedDashboardViewModel>>(Dashboards));
            });
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            return ApiAction(() =>
            {
                var Dashboard = _DashboardService.Read(Id);

                if (Dashboard == null)
                {
                    return NotFound(Id);
                }

                return Accepted(_mapper.Map<ReadedDashboardViewModel>(Dashboard));
            });
        }

        // POST api/dashboard
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody] CreateDashboardViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDashboardContract>(vm);
                var returnContract = _DashboardService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDashboardViewModel>(returnContract));
            });
        }

        // PUT api/dashboard/5
        // Mutation
        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody]UpdateDashboardViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDashboardContract>(vm);
                contract.Id = Id;
                _DashboardService.Update(contract);

                return Accepted(new { Id });
            });
        }

        // DELETE api/dashboard/5
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            return ApiAction(() =>
            {
                _DashboardService.Delete(Id);
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