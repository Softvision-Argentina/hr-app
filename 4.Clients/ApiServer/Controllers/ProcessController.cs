using ApiServer.Contracts.Process;
using AutoMapper;
using Core;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain.Services.Contracts.Process;
using Microsoft.AspNetCore.Authorization;
using Domain.Services.Interfaces.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    /// <summary>
    /// Controller that manages processes
    /// Contains action method for approve or disapprove
    /// </summary>
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : BaseController<ProcessController>
    {
        private readonly IProcessService _processService;
        private readonly IMapper _mapper;
        private INotificationRepository _notificationRepository;

        public ProcessController(IProcessService processService,
            ILog<ProcessController> logger, IMapper mapper)
            : base(logger)
        {
            _processService = processService;
            _mapper = mapper;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
              var processes = _processService.List();

                return Accepted(_mapper.Map<List<ReadedProcessViewModel>>(processes));
            });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var process = _processService.Read(id);

                return Accepted(_mapper.Map<ReadedProcessViewModel>(process));
            });
        }
        
        [HttpGet("com/{community}")]
        public IActionResult GetProcessesByCommunity(string community)
        {
            return ApiAction(() =>
            {
                var process = _processService.GetProcessesByCommunity(community);

                return Accepted(_mapper.Map<List<ReadedProcessViewModel>>(process));
            });
        }

        // POST api/<controller>
        // Create
        [HttpPost]
        public IActionResult Post([FromBody]CreateProcessViewModel createProcessViewModel)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateProcessContract>(createProcessViewModel);
                var returnContract = _processService.Create(contract);
                return Created("Get", _mapper.Map<CreatedProcessViewModel>(returnContract));
            });
        }

        // PUT api/<controller>/5
        // Update
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateProcessViewModel updateProcessContract)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateProcessContract>(updateProcessContract);
                contract.Id = id;

                _processService.Update(contract);

                var processes = _processService.List();

                return Accepted(_mapper.Map<List<ReadedProcessViewModel>>(processes));
            });
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _processService.Delete(id);

                return Accepted();
            });
        }

        // DELETE api/dummies/5
        [HttpPost("Approve")]
        public IActionResult Approve([FromBody]int id)
        {
            return ApiAction(() =>
            {
                _processService.Approve(id);

                return Accepted();
            });
        }

        // DELETE api/dummies/5
        [HttpPost("Reject")]
        public IActionResult Reject([FromBody] RejectProcessViewModel processVM)
        {
            return ApiAction(() =>
            {
                _processService.Reject(processVM.Id, processVM.RejectionReason);

                return Accepted();
            });
        }

        // GET api/<controller>/5
        [HttpGet("{candidateId}")]
        public IActionResult GetActiveProcessByCandidate(int candidateId)
        {
            return ApiAction(() =>
            {
                var process = _processService.GetActiveByCandidateId(candidateId);

                return Accepted(_mapper.Map<IEnumerable<ReadedProcessViewModel>>(process));
            });
        }
    }
}
