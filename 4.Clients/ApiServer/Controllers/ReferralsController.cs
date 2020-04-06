using System.Collections.Generic;
using ApiServer.Contracts.Candidates;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Domain.Model;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralsController : BaseController<ReferralsController>
    {
        ICandidateService _candidateService;
        private IMapper _mapper;

        public ReferralsController(ICandidateService candidateService,
                                 ILog<ReferralsController> logger,
                                 IMapper mapper) : base(logger)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateCandidateViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateCandidateContract>(vm);
                var returnContract = _candidateService.Create(contract);

                return Created("Get", _mapper.Map<CreatedCandidateViewModel>(returnContract));
            });
        }
    }
}
