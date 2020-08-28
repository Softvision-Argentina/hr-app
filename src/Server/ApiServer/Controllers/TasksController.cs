// <copyright file="TasksController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Task;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Task;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController<TasksController>
    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public TasksController(
            ITaskService taskService,
            ILog<TasksController> logger,
            IMapper mapper) : base(logger)
        {
            this.taskService = taskService;
            this.mapper = mapper;
        }

        [HttpGet("{userId}")]
        public IActionResult GetTaskByUserId(int userId)
        {
            return this.ApiAction(() =>
            {
                var tasks = this.taskService.List(userId);

                return this.Accepted(this.mapper.Map<List<ReadedTaskViewModel>>(tasks));
            });
        }

        [HttpGet("getById/{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var task = this.taskService.Read(id);

                if (task == null)
                {
                    return this.NotFound(id);
                }

                var vm = this.mapper.Map<ReadedTaskViewModel>(task);
                return this.Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTaskViewModel createTaskVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateTaskContract>(createTaskVm);
                var returnContract = this.taskService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedTaskViewModel>(returnContract));
            });
        }

        // Todo: review this code
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateTaskViewModel updateTaskVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateTaskContract>(updateTaskVm);
                contract.Id = id;
                contract.IsNew = false; // Damos por sentado que ya no es nueva para que desaparezca el icono. El usuario la editó
                this.taskService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.taskService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }

        // Todo: convention over configuration
        [HttpPost("Approve/{id}")]
        public IActionResult Approve(int id)
        {
            return this.ApiAction(() =>
            {
                this.taskService.Approve(id);
                return this.Accepted(new { id });
            });
        }

        [HttpGet("GetByUser/{UserEmail}")]
        public IActionResult GetByUser(string userEmail)
        {
            return this.ApiAction(() =>
            {
                var tasks = this.taskService.ListByUser(userEmail);
                return this.Accepted(this.mapper.Map<List<ReadedTaskViewModel>>(tasks));
            });
        }
    }
}