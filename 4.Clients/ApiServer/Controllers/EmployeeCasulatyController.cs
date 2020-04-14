using ApiServer.Contracts.EmployeeCasualty;
using AutoMapper;
using Core;
using Domain.Services.Contracts.EmployeeCasualty;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCasualtyController : BaseController<EmployeeCasualtyController>
    {
        private readonly IEmployeeCasualtyService _employeeCasualtyService;
        private readonly IMapper _mapper;

        public EmployeeCasualtyController(
            IEmployeeCasualtyService employeeCasualtyService, 
            ILog<EmployeeCasualtyController> logger, 
            IMapper mapper) : base(logger)
        {
            _employeeCasualtyService = employeeCasualtyService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var employeeCasualty = _employeeCasualtyService.List();

                return Accepted(_mapper.Map<List<ReadedEmployeeCasualtyViewModel>>(employeeCasualty));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var employeeCasualty = _employeeCasualtyService.Read(id);

                if (employeeCasualty == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedEmployeeCasualtyViewModel>(employeeCasualty));
            });
        }

       [HttpPost]
        public IActionResult Post([FromBody]CreateEmployeeCasualtyViewModel createEmployeeCasualtyVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateEmployeeCasualtyContract>(createEmployeeCasualtyVm);
                var returnContract = _employeeCasualtyService.Create(contract);

                return Created("Get", _mapper.Map<CreatedEmployeeCasualtyViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateEmployeeCasualtyViewModel updateEmployeeCasualtyVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateEmployeeCasualtyContract>(updateEmployeeCasualtyVm);
                contract.Id = id;
                _employeeCasualtyService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _employeeCasualtyService.Delete(id);
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