using ApiServer.Contracts.Stage;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Stage;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    public class ProcessStageController : BaseController<ProcessStageController>
    {
        private readonly IProcessStageService _processStageService;
        private readonly IMapper _mapper;

        public ProcessStageController(
            IProcessStageService processStageService,
            ILog<ProcessStageController> logger,
            IMapper mapper)
            : base(logger)
        {
            _processStageService = processStageService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var stages = _processStageService.List();

                return Accepted(_mapper.Map<List<ReadedStageViewModel>>(stages));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var stage = _processStageService.Read(id);

                return Accepted(_mapper.Map<ReadedStageViewModel>(stage));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateStageViewModel createStageVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateStageContract>(createStageVm);
                var returnContract = _processStageService.Create(contract);

                return Created("Get", _mapper.Map<CreatedStageViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateStageViewModel updateStageVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateStageContract>(updateStageVm);
                contract.Id = id;

                _processStageService.Update(contract);

                return Accepted();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _processStageService.Delete(id);

                return Accepted();
            });
        }
    }
}
