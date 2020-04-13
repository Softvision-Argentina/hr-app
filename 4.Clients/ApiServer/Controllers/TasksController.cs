using System.Collections.Generic;
using ApiServer.Contracts.Task;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Task;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController<TasksController>
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(
            ITaskService taskService,
            ILog<TasksController> logger,
            IMapper mapper) : base(logger)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var tasks = _taskService.List();

                return Accepted(_mapper.Map<List<ReadedTaskViewModel>>(tasks));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var task = _taskService.Read(id);

                if (task == null)
                {
                    return NotFound(id);
                }

                var vm = _mapper.Map<ReadedTaskViewModel>(task);
                return Accepted(vm);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateTaskViewModel createTaskVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateTaskContract>(createTaskVm);
                var returnContract = _taskService.Create(contract);

                return Created("Get", _mapper.Map<CreatedTaskViewModel>(returnContract));
            });
        }

        //Todo: review this code
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateTaskViewModel updateTaskVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateTaskContract>(updateTaskVm);
                contract.Id = id;
                contract.IsNew = false; //Damos por sentado que ya no es nueva para que desaparezca el icono. El usuario la editó
                _taskService.Update(contract);
                
                return Accepted(new { id });
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _taskService.Delete(id);
                return Accepted();
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }

        //Todo: convention over configuration
        [HttpPost("Approve/{id}")]
        public IActionResult Approve(int id)
        {
            return ApiAction(() =>
            {
                _taskService.Approve(id);
                return Accepted(new { id });
            });
        }

        [HttpGet("GetByConsultant/{consultantEmail}")]
        public IActionResult GetByConsultant(string consultantEmail)
        {
            return ApiAction(() =>
            {
                var tasks = _taskService.ListByConsultant(consultantEmail);
                return Accepted(_mapper.Map<List<ReadedTaskViewModel>>(tasks));
            });
        }
    }
}