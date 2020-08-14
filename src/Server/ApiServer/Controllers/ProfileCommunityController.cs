namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Core;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using ApiServer.Contracts.ProfileByCommunity;
    using Domain.Services.Contracts.ProfileByCommunity;

    [Route("api/[controller]")]
    [ApiController]
    public class ProfileCommunityController : BaseController<ProfileCommunityController>
    {
        private readonly IProfileCommunityService _profileByCommunityService;
        private readonly IMapper _mapper;

        public ProfileCommunityController(
            IProfileCommunityService profileByCommunityService,
            ILog<ProfileCommunityController> logger,
            IMapper mapper) : base(logger)
        {
            _profileByCommunityService = profileByCommunityService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var profilebycomm = _profileByCommunityService.Get(id);

                if (profilebycomm == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<List<ReadedProfileCommunityViewModel>>(profilebycomm);

                return Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateProfileCommunityViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateProfileCommunityContract>(vm);
                var returnContract = _profileByCommunityService.Create(contract);

                return Created("Get", _mapper.Map<CreatedProfileCommunityViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateProfileCommunityViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateProfileCommunityContract>(vm);
                contract.Id = id;
                _profileByCommunityService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _profileByCommunityService.Delete(id);
                return Accepted();
            });
        }
    }
}