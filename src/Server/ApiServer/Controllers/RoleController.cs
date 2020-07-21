using System.Collections.Generic;
using ApiServer.Contracts.Role;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Role;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : BaseController<RoleController>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(
            IRoleService roleService,
            ILog<RoleController> logger,
            IMapper mapper) : base(logger)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var roles = _roleService.List();

                return Accepted(_mapper.Map<List<ReadedRoleViewModel>>(roles));
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody]CreateRoleViewModel createRoleVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<CreateRoleContract>(createRoleVm);
                var returnContract = _roleService.Create(contract);

                return Created("Get", _mapper.Map<CreatedRoleViewModel>(returnContract));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return ApiAction(() =>
            {
                _roleService.Delete(id);
                return Accepted();
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateRoleViewModel updateRoleVm)
        {
            return ApiAction(() =>
            {
                var contract = _mapper.Map<UpdateRoleContract>(updateRoleVm);
                contract.Id = id;
                _roleService.Update(contract);

                return Accepted(new { id });
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return ApiAction(() =>
            {
                var role = _roleService.Read(id);

                if (role == null)
                {
                    return NotFound(id);
                }

                return Accepted(_mapper.Map<ReadedRoleViewModel>(role));
            });
        }
    }
}