using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Contracts.Task;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Task;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController<TasksController>
    {
        ITaskService _taskService;
        private IMapper _mapper;

        public TasksController(ITaskService taskService,
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

        // GET api/tasks/5
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

        // POST api/tasks
        // Creation
        [HttpPost]
        public IActionResult Post([FromBody]CreateTaskViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateTaskContract>(vm);
                var returnContract = _taskService.Create(contract);

                return Created("Get", _mapper.Map<CreatedTaskViewModel>(returnContract));
            });
        }


        // PUT api/tasks/5
        // Mutation
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UpdateTaskViewModel vm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateTaskContract>(vm);
                contract.Id = id;
                contract.IsNew = false; //Damos por sentado que ya no es nueva para que desaparezca el icono. El usuario la editó
                _taskService.Update(contract);
                
                return Accepted(new { id });
            });
        }

        // DELETE api/tasks/5
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

        [HttpPost("Approve/{id}")]
        public IActionResult Approve(int id)
        {
            return ApiAction(() =>
            {
                _taskService.Approve(id);
                return Accepted(new { id });
            });
        }

        [HttpGet("GetByUser/{UserEmail}")]
        public IActionResult GetByUser(string UserEmail)
        {
            return ApiAction(() =>
            {
                var tasks = _taskService.ListByUser(UserEmail);
                return Accepted(_mapper.Map<List<ReadedTaskViewModel>>(tasks));
            });
        }
    }
}