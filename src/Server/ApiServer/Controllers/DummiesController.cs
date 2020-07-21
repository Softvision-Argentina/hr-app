using ApiServer.Contracts.Seed;
using ApiServer.Security;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Seed;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiServer.Controllers
{
    //TODO:do we use actually this controller ? or we can remove it.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DummiesController : BaseController<DummiesController>
    {
        private readonly IDummyService _dummyService;
        private readonly IMapper _mapper;

        public DummiesController(
            IDummyService dummyService,
            ILog<DummiesController> logger,
            IMapper mapper)
            : base(logger)
        {
            _dummyService = dummyService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = SecurityClaims.CAN_LIST_DUMMY)]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var dummies = _dummyService.List();

                return Accepted(_mapper.Map<List<ReadedDummyViewModel>>(dummies));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return ApiAction(() =>
            {
                var dummy = _dummyService.Read(id);

                if (dummy == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedDummyViewModel>(dummy));
            });
        }

        // POST api/dummies
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody]CreateDummyViewModel createDummyVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateDummyContract>(createDummyVm);
                var returnContract = _dummyService.Create(contract);

                return Created("Get", _mapper.Map<CreatedDummyViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]UpdateDummyViewModel updateDummyVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateDummyContract>(updateDummyVm);
                contract.Id = id;
                _dummyService.Update(contract);

                return Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return ApiAction(() =>
            {
                _dummyService.Delete(id);
                return Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new PingViewModel { Status = "OK" });
        }

    }
}
