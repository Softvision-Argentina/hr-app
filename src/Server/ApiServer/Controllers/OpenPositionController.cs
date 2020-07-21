using System.Collections.Generic;
using AutoMapper;
using Core;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Domain.Model;
using ApiServer.Contracts.OpenPosition;
using Domain.Services.Contracts.OpenPositions;

namespace ApiServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OpenPositionController : BaseController<OpenPositionController>
    {
        private readonly IOpenPositionService _openPositionService;
        private readonly IMapper _mapper;

        public OpenPositionController(
            IOpenPositionService openPositionService,
            ILog<OpenPositionController> logger,
            IMapper mapper) : base(logger)
        {
            _openPositionService = openPositionService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var openPositions = _openPositionService.Get();

                return Accepted(_mapper.Map<List<ReadedOpenPositionViewModel>>(openPositions));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var position = _openPositionService.GetById(id);

                if (position == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<ReadedOpenPositionViewModel>(position);

                return Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateOpenPositionViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateOpenPositionContract>(vm);
                var returnContract = _openPositionService.Create(contract);

                return Created("Get", _mapper.Map<CreatedOpenPositionViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateOpenPositionViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateOpenPositionContract>(vm);
                contract.Id = id;
                _openPositionService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _openPositionService.Delete(id);
                return Accepted();
            });
        }
    }
}