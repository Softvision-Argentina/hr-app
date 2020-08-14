namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Core;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using ApiServer.Contracts.ProfileByCommunity;
    using Domain.Services.Contracts.ProfileByCommunity;
    using Domain.Services.Contracts.SkillProfile;

    [Route("api/[controller]")]
    [ApiController]
    public class SkillProfileController : BaseController<SkillProfileController>
    {
        private readonly ISkillProfileService _skillProfileService;
        private readonly IMapper _mapper;

        public SkillProfileController(
            ISkillProfileService skillProfileService,
            ILog<SkillProfileController> logger,
            IMapper mapper) : base(logger)
        {
            _skillProfileService = skillProfileService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var skillProfile = _skillProfileService.GetAll(id);

                if (skillProfile == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<List<ReadedSkillProfileViewModel>>(skillProfile);

                return Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateSkillProfileViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateSkillProfileContract>(vm);
                var returnContract = _skillProfileService.Create(contract);

                return Created("Get", _mapper.Map<CreatedSkillProfileViewModel>(returnContract));
            });
        }

        [HttpPut("{profileId}/{skillId}")]
        public IActionResult Put(int profileId, int skillId, [FromBody]UpdateSkillProfileViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateSkillProfileContract>(vm);
                _skillProfileService.Update(profileId, skillId, contract);

                return Accepted(new { profileId });
            });
        }

        [HttpDelete("{profileId}/{skillId}")]
        public IActionResult Delete(int profileId, int skillId)
        {
            return ApiAction(() =>
            {
                _skillProfileService.Delete(profileId, skillId);
                return Accepted();
            });
        }
    }
}