using ApiServer.Contracts.DaysOff;
using AutoMapper;
using Core;
using Domain.Services.Contracts.DaysOff;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysOffController : BaseController<DaysOffController>
    {
        private readonly IDaysOffService _daysOffService;
        private readonly IMapper _mapper;

        public DaysOffController(
            IDaysOffService daysOffService,
            ILog<DaysOffController> logger,
            IMapper mapper) : base(logger)
        {
            _daysOffService = daysOffService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var daysOff = _daysOffService.List();

                return Accepted(_mapper.Map<List<ReadedDaysOffViewModel>>(daysOff));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var daysOff = _daysOffService.Read(id);

                if (daysOff == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedDaysOffViewModel>(daysOff));
            });
        }

        [HttpGet("GetByDni")]
        public IActionResult GetByDni([FromQuery]int dni)
        {
            return ApiAction(() =>
            {
                var daysOff = _daysOffService.ReadByDni(dni);

                if ((daysOff == null) || (daysOff.Count() == 0))
                {
                    return NotFound(dni);
                }

                return Accepted(_mapper.Map<List<ReadedDaysOffViewModel>>(daysOff));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateDaysOffViewModel createDaysOffVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDaysOffContract>(createDaysOffVm);
                var returnContract = _daysOffService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDaysOffViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateDaysOffViewModel updateDaysOffVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDaysOffContract>(updateDaysOffVm);
                contract.Id = id;
                _daysOffService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _daysOffService.Delete(id);
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