using ApiServer.Contracts.CompanyCalendar;
using AutoMapper;
using Core;
using Domain.Services.Contracts.CompanyCalendar;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyCalendarController : BaseController<CompanyCalendarController>
    {
        private readonly ICompanyCalendarService _companyCalendarService;
        private readonly IMapper _mapper;

        public CompanyCalendarController(
            ICompanyCalendarService companyCalendarService, 
            ILog<CompanyCalendarController> logger, 
            IMapper mapper) : base(logger)
        {
            _companyCalendarService = companyCalendarService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var companyCalendar = _companyCalendarService.List();

                return Accepted(_mapper.Map<List<ReadedCompanyCalendarViewModel>>(companyCalendar));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var companyCalendar = _companyCalendarService.Read(id);

                if (companyCalendar == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedCompanyCalendarViewModel>(companyCalendar));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateCompanyCalendarViewModel createCompanyCalendarVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCompanyCalendarContract>(createCompanyCalendarVm);
                var returnContract = _companyCalendarService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCompanyCalendarViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateCompanyCalendarViewModel updateCompanyCalendarVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateCompanyCalendarContract>(updateCompanyCalendarVm);
                contract.Id = id;
                _companyCalendarService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _companyCalendarService.Delete(id);
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