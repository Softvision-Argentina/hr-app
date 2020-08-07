// <copyright file="UserController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using ApiServer.Contracts.User;
    using AutoMapper;
    using Core;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(
            IUserService userService,
            ILog<UserController> logger,
            IMapper mapper) : base(logger)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        // Todo: convention over configuration
        [HttpGet("GetRoleByUserName/{username}")]
        public IActionResult GetRoleByUserName(string username)
        {
            return this.ApiAction(() =>
            {
                var userRole = this.userService.GetUserRole(username);

                if (userRole == null)
                {
                    return this.NotFound(username);
                }

                var vm = this.mapper.Map<ReadedUserRoleViewModel>(userRole);
                return this.Accepted(vm);
            });
        }

        [HttpGet("GetFilteredForTech")]
        public IActionResult GetFilteredForTech()
        {
            return this.ApiAction(() =>
            {
                var users = this.userService.GetFilteredForTech();

                if (users == null)
                {
                    return this.NotFound();
                }

                return this.Accepted(users);
            });
        }

        [HttpGet("GetFilteredForHr")]
        public IActionResult GetFilteredForHr()
        {
            return this.ApiAction(() =>
            {
                var users = this.userService.GetFilteredForHr();

                if (users == null)
                {
                    return this.NotFound();
                }

                return this.Accepted(users);
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.ApiAction(() =>
            {
                var users = this.userService.GetAll();

                if (users == null)
                {
                    return this.NotFound();
                }

                var vm = users;
                return this.Accepted(vm);
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }
    }
}