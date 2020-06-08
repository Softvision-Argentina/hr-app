using System.Collections.Generic;
using ApiServer.Contracts.Interview;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Interview;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Domain.Model;

namespace ApiServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : BaseController<InterviewsController>
    {

        private readonly IInterviewService _interviewService;
        private readonly IMapper _mapper;


        public InterviewsController(
            IInterviewService interviewService,
            ILog<InterviewsController> logger,
            IMapper mapper) : base(logger)
        {
            _interviewService = interviewService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var interviews = _interviewService.List();

                return Accepted(_mapper.Map<List<ReadedInterviewViewModel>>(interviews));

            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var interview = _interviewService.Read(id);

                if (interview == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<ReadedInterviewViewModel>(interview);
                return Accepted(vm);
            });
        }


        [HttpPost]
        public IActionResult Post([FromBody]CreateInterviewViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateInterviewContract>(vm);
                var returnContract = _interviewService.Create(contract);

                return Created("Get", _mapper.Map<CreatedInterviewViewModel>(returnContract));
            });
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateInterviewViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateInterviewContract>(vm);
                contract.Id = id;
                _interviewService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _interviewService.Delete(id);
                return Accepted();
            });
        }

    }

}