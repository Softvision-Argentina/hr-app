using System.Collections.Generic;
using ApiServer.Contracts;
using AutoMapper;
using Core;
using Domain.Services.Contracts;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeclineReasonController : BaseController<DeclineReasonController>
    {
        private readonly IDeclineReasonService _declineReasonService;
        private readonly IMapper _mapper;

        public DeclineReasonController(
            IDeclineReasonService declineReasonService,
            ILog<DeclineReasonController> logger,
            IMapper mapper) : base(logger)
        {
            _declineReasonService = declineReasonService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var declineReasons = _declineReasonService.List();

                return Accepted(_mapper.Map<List<ReadedDeclineReasonViewModel>>(declineReasons));
            });
        }

        [HttpGet("Named")]
        public IActionResult GetNamed()
        {
            return ApiAction(() =>
            {
                var declineReasons = _declineReasonService.ListNamed();

                return Accepted(_mapper.Map<List<ReadedDeclineReasonViewModel>>(declineReasons));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var skillTpe = _declineReasonService.Read(id);

                if (skillTpe == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedDeclineReasonViewModel>(skillTpe));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateDeclineReasonViewModel createDeclineReasonVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDeclineReasonContract>(createDeclineReasonVm);
                var returnContract = _declineReasonService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDeclineReasonViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateDeclineReasonViewModel updateDeclineReasonVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDeclineReasonContract>(updateDeclineReasonVm);
                contract.Id = id;
                _declineReasonService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _declineReasonService.Delete(id);
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