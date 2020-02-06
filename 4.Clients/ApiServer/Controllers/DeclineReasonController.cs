using System.Collections.Generic;
using ApiServer.Contracts;
using AutoMapper;
using Core;
using Domain.Services.Contracts;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeclineReasonController : BaseController<DeclineReasonController>
    {
        IDeclineReasonService _declineReasonService;
        private IMapper _mapper;

        public DeclineReasonController(IDeclineReasonService declineReasonService, ILog<DeclineReasonController> logger, IMapper mapper) : base(logger)
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

        // GET api/declineReason/5
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

        // POST api/declineReason
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody]CreateDeclineReasonViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDeclineReasonContract>(vm);
                var returnContract = _declineReasonService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDeclineReasonViewModel>(returnContract));
            });
        }

        // PUT api/declineReason/5
        // Mutation
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateDeclineReasonViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDeclineReasonContract>(vm);
                contract.Id = id;
                _declineReasonService.Update(contract);

                return Accepted(new { id });
            });
        }

        // DELETE api/declineReason/5
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