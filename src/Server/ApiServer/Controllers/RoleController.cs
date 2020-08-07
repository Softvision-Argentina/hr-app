// <copyright file="RoleController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Role;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Role;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RoleController : BaseController<RoleController>
    {
        private readonly IRoleService roleService;
        private readonly IMapper mapper;

        public RoleController(
            IRoleService roleService,
            ILog<RoleController> logger,
            IMapper mapper) : base(logger)
        {
            this.roleService = roleService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var roles = this.roleService.List();

                return this.Accepted(this.mapper.Map<List<ReadedRoleViewModel>>(roles));
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody] CreateRoleViewModel createRoleVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<CreateRoleContract>(createRoleVm);
                var returnContract = this.roleService.Create(contract);

                return this.Created("Get", this.mapper.Map<CreatedRoleViewModel>(returnContract));
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return this.ApiAction(() =>
            {
                this.roleService.Delete(id);
                return this.Accepted();
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateRoleViewModel updateRoleVm)
        {
            return this.ApiAction(() =>
            {
                var contract = this.mapper.Map<UpdateRoleContract>(updateRoleVm);
                contract.Id = id;
                this.roleService.Update(contract);

                return this.Accepted(new { id });
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.ApiAction(() =>
            {
                var role = this.roleService.Read(id);

                if (role == null)
                {
                    return this.NotFound(id);
                }

                return this.Accepted(this.mapper.Map<ReadedRoleViewModel>(role));
            });
        }
    }
}