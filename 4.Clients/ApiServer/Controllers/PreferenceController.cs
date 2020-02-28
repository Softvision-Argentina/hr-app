using System.Collections.Generic;
using ApiServer.Contracts.Preference;
using ApiServer.Contracts.User;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Preference;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : BaseController<PreferenceController>
    {
        IPreferenceService _preferenceService;
        private IMapper _mapper;

        public PreferenceController(IPreferenceService preferenceService,
                         ILog<PreferenceController> logger,
                         IMapper mapper) : base(logger)
        {
            _preferenceService = preferenceService;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var preferences = _preferenceService.List();

                return Accepted(_mapper.Map<List<ReadedPreferenceViewModel>>(preferences));
            });
        }

        // GET api/Preference/1
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            return ApiAction(() =>
            {
                var Preference = _preferenceService.Read(Id);

                if (Preference == null)
                {
                    return NotFound(Id);
                }

                return Accepted(_mapper.Map<ReadedPreferenceViewModel>(Preference));
            });
        }

        // PUT api/Preference/1
        [HttpPut("{Id}")]
        public IActionResult Put(int Id, [FromBody]UpdatePreferenceViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdatePreferenceContract>(vm);
                contract.Id = Id;

                _preferenceService.Update(contract);

                return Accepted(new { Id });
            });
        }

        // POST api/preference
        [HttpPost]
        public IActionResult Post([FromBody] CreatePreferenceViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreatePreferenceContract>(vm);
                var returnContract = _preferenceService.Create(contract);

                return Created("Get", _mapper.Map<CreatedPreferenceViewModel>(returnContract));
            });
        }

    }
}