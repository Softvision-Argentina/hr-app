using System.Collections.Generic;
using ApiServer.Contracts.Skills;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Skill;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : BaseController<SkillsController>
    {
        private readonly ISkillService _skillService;
        private readonly IMapper _mapper;

        public SkillsController(
            ISkillService skillService, 
            ILog<SkillsController> logger, 
            IMapper mapper): base(logger)
        {
            _skillService = skillService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var skills = _skillService.List();

                return Accepted(_mapper.Map<List<ReadedSkillViewModel>>(skills));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var skill = _skillService.Read(id);

                if (skill == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedSkillViewModel>(skill));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateSkillViewModel createSkillsVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateSkillContract>(createSkillsVm);
                var returnContract = _skillService.Create(contract);

                return Created("Get", _mapper.Map<CreatedSkillViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateSkillViewModel updateSkillsVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateSkillContract>(updateSkillsVm);
                contract.Id = id;
                _skillService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _skillService.Delete(id);
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