using ApiServer.Contracts.Employee;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Employee;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<EmployeesController>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeesController(
            IEmployeeService employeeService,
            ILog<EmployeesController> logger,
            IMapper mapper) : base(logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return ApiAction(() =>
            {
                var employees = _employeeService.List();
                return Accepted(_mapper.Map<List<ReadedEmployeeViewModel>>(employees));
            });
        }

        //TODO: we could simplify this base on convention over configuration same for the rest of the endpoints
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            return ApiAction(() =>
            {
                var employee = _employeeService.GetById(id);
                return Accepted(_mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpGet("GetByDni")]
        public IActionResult GetByDNI([FromQuery] int dni)
        {
            return ApiAction(() =>
            {
                var employee = _employeeService.GetByDNI(dni);
                return Accepted(_mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail([FromQuery] string email)
        {
            return ApiAction(() =>
            {
                var employee = _employeeService.GetByEmail(email);
                return Accepted(_mapper.Map<ReadedEmployeeViewModel>(employee));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _employeeService.Delete(id);
                return Accepted();
            });
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody]UpdateEmployeeViewModel updateEmployeeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateEmployeeContract>(updateEmployeeVm);
                _employeeService.UpdateEmployee(contract);

                return Accepted();
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody]CreateEmployeeViewModel createEmployeeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateEmployeeContract>(createEmployeeVm);
                var returnContract = _employeeService.Create(contract);

                return Created("Get", _mapper.Map<CreatedEmployeeViewModel>(returnContract));
            });
        }
    }
}
