using ApiServer.Contracts.HireProjection;
using AutoMapper;
using Core;
using Domain.Services.Contracts.HireProjection;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HireProjectionsController : BaseController<HireProjectionsController>
    {
        private readonly IHireProjectionService _hireProjectionService;
        private readonly IMapper _mapper;

        public HireProjectionsController(
            IHireProjectionService hireProjectionService, 
            ILog<HireProjectionsController> logger, 
            IMapper mapper) : base(logger)
        {
            _hireProjectionService = hireProjectionService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var hireProjections = _hireProjectionService.List();

                return Accepted(_mapper.Map<List<ReadedHireProjectionViewModel>>(hireProjections));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var hireProjection = _hireProjectionService.Read(id);

                if (hireProjection == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedHireProjectionViewModel>(hireProjection));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateHireProjectionViewModel createHireProjectVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateHireProjectionContract>(createHireProjectVm);
                var returnContract = _hireProjectionService.Create(contract);

                return Created("Get", _mapper.Map<CreatedHireProjectionViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateHireProjectionViewModel updateHireProjectionVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateHireProjectionContract>(updateHireProjectionVm);
                contract.Id = id;
                _hireProjectionService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _hireProjectionService.Delete(id);
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