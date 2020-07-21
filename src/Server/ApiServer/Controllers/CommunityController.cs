using System.Collections.Generic;
using ApiServer.Contracts.Community;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Community;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : BaseController<CommunityController>
    {
        private readonly ICommunityService _communityService;
        private readonly IMapper _mapper;

        public CommunityController(
            ICommunityService communityService,
            ILog<CommunityController> logger,
            IMapper mapper
            ) : base(logger)
        {
            _communityService = communityService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var communities = _communityService.List();

                return Accepted(_mapper.Map<List<ReadedCommunityViewModel>>(communities));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var community = _communityService.Read(id);

                if (community == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedCommunityViewModel>(community));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateCommunityViewModel createCommunityVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCommunityContract>(createCommunityVm);
                var returnContract = _communityService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCommunityViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateCommunityViewModel updateCommunityVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateCommunityContract>(updateCommunityVm);
                contract.Id = id;
                _communityService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _communityService.Delete(id);
                return Accepted();
            });
        }

        //TODO: esto es un health check? no podemos tenerlo en un solo controller.es necesario que este en todos?
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}