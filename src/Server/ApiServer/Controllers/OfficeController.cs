using System.Collections.Generic;
using ApiServer.Contracts.Office;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Office;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OfficeController : BaseController<OfficeController> {

        private readonly IOfficeService _officeService;
        private readonly IMapper _mapper;

        public OfficeController(
            IOfficeService officeService,
            ILog<OfficeController> logger,
            IMapper mapper): base(logger)
        {
            _officeService = officeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var offices = _officeService.List();

                return Accepted(_mapper.Map<List<ReadedOfficeViewModel>>(offices));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var office = _officeService.Read(id);

                if (office == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedOfficeViewModel>(office));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateOfficeViewModel createOfficeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateOfficeContract>(createOfficeVm);
                var returnContract = _officeService.Create(contract);

                return Created("Get", _mapper.Map<CreatedOfficeViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateOfficeViewModel updateOfficeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateOfficeContract>(updateOfficeVm);
                contract.Id = id;
                _officeService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _officeService.Delete(id);
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