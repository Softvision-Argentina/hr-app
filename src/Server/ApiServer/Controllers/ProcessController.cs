// <copyright file="ProcessController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.Process;
    using ApiServer.Contracts.User;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Process;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller that manages processes
    /// Contains action method for approve or disapprove.
    /// </summary>
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : BaseController<ProcessController>
    {
        private readonly IProcessService processService;
        private readonly IMapper mapper;

        public ProcessController(IProcessService processService, ILog<ProcessController> logger, IMapper mapper)
            : base(logger)
        {
            this.processService = processService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var processes = this.processService.List();

                return this.Accepted(this.mapper.Map<List<ReadedProcessViewModel>>(processes));
            });
        }

        [HttpGet("tableView")]
        public IActionResult GetTableView()
        {
            return this.ApiAction(() =>
            {
                var processes = this.processService.List();

                return this.Accepted(this.mapper.Map<List<TableProcessViewModel>>(processes));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var process = this.processService.Read(id);

                return this.Accepted(this.mapper.Map<ReadedProcessViewModel>(process));
            });
        }

        [HttpGet("com/{community}")]
        public IActionResult GetProcessesByCommunity(string community)
        {
            return this.ApiAction(() =>
            {
                var process = this.processService.GetProcessesByCommunity(community);

                return this.Accepted(this.mapper.Map<List<ReadedProcessViewModel>>(process));
            });
        }

        [HttpGet("owner/{id}")]
        public IActionResult GetProcessesForOwner(int id)
        {
            return this.ApiAction(() =>
            {
                var processes = this.processService.List().ToList().Where(process => process.UserDelegateId == id || process.UserOwnerId == id);

                return this.Accepted(this.mapper.Map<List<ReadedProcessViewModel>>(processes));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateProcessViewModel createProcessViewModel)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateProcessContract>(createProcessViewModel);
                var returnContract = this.processService.Create(contract);
                return this.Created("Get", this.mapper.Map<CreatedProcessViewModel>(returnContract));
            });
        }

        [HttpPost("tableView")]
        public IActionResult PostTableView([FromBody] CreateProcessViewModel createProcessViewModel)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateProcessContract>(createProcessViewModel);
                var returnContract = this.processService.Create(contract);
                return this.Created("Get", this.mapper.Map<TableProcessViewModel>(returnContract));
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateProcessViewModel updateProcessContract)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateProcessContract>(updateProcessContract);
                contract.Id = id;
                var returnContract = this.processService.Update(contract);
                return this.Created("Get", this.mapper.Map<TableProcessViewModel>(returnContract));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.processService.Delete(id);

                return this.Accepted();
            });
        }

        // TODO: cant we use de convention over configuration?
        [HttpPost("Approve")]
        public IActionResult Approve([FromBody] int id)
        {
            return this.ApiAction(() =>
            {
                var returnContract = this.processService.Approve(id);
                return this.Accepted(this.mapper.Map<TableProcessViewModel>(returnContract));
            });
        }

        [HttpPost("Reactivate")]
        public IActionResult Reactivate([FromBody] int id)
        {
            return this.ApiAction(() =>
            {
                var process = this.processService.Reactivate(id);

                return this.Accepted(this.mapper.Map<TableProcessViewModel>(process));
            });
        }

        [HttpPost("Reject")]
        public IActionResult Reject([FromBody] RejectProcessViewModel rejectProcessVm)
        {
            return this.ApiAction(() =>
            {
                this.processService.Reject(rejectProcessVm.Id, rejectProcessVm.RejectionReason);

                return this.Accepted();
            });
        }

        [HttpGet("candidate/{candidateId}")]
        public IActionResult GetActiveProcessByCandidate(int candidateId)
        {
            return this.ApiAction(() =>
            {
                var process = this.processService.GetActiveByCandidateId(candidateId);

                return this.Accepted(this.mapper.Map<IEnumerable<ReadedProcessViewModel>>(process));
            });
        }

        [HttpGet("DeletedProcesses")]
        public IActionResult GetDeletedProcesses()
        {
            return this.ApiAction(() =>
            {
                var process = this.processService.GetDeletedProcesses();

                return this.Accepted(this.mapper.Map<IEnumerable<ReadedProcessViewModel>>(process));
            });
        }
    }
}
