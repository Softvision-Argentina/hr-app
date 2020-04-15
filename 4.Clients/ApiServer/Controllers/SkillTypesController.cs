using System.Collections.Generic;
using ApiServer.Contracts.SkillType;
using AutoMapper;
using Core;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillTypesController : BaseController<SkillTypesController>
    {
        private readonly ISkillTypeService _skillTypeService;
        private readonly IMapper _mapper;

        public SkillTypesController(
            ISkillTypeService skillTypeService, 
            ILog<SkillTypesController> logger, 
            IMapper mapper) : base(logger)
        {
            _skillTypeService = skillTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var skillsTypes = _skillTypeService.List();

                return Accepted(_mapper.Map<List<ReadedSkillTypeViewModel>>(skillsTypes));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var skillTpe = _skillTypeService.Read(id);

                if (skillTpe == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedSkillTypeViewModel>(skillTpe));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateSkillTypeViewModel createSkillTypeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateSkillTypeContract>(createSkillTypeVm);
                var returnContract = _skillTypeService.Create(contract);

                return Created("Get", _mapper.Map<CreatedSkillTypeViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateSkillTypeViewModel updateSkillTypeVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateSkillTypeContract>(updateSkillTypeVm);
                contract.Id = id;
                _skillTypeService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _skillTypeService.Delete(id);
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