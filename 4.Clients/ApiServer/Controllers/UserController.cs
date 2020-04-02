using System.Collections.Generic;
using ApiServer.Contracts.User;
using AutoMapper;
using Core;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        IUserService _userService;
        private IMapper _mapper;

        public UserController(IUserService userService,
                                 ILog<UserController> logger,
                                 IMapper mapper) : base(logger)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET api/user/john.doe@softvision.com
        // returns the role
        [HttpGet("GetRoleByUserName/{username}")]
        public IActionResult GetRoleByUserName(string username)
        {
            return ApiAction(() =>
            {
                var userRole = _userService.GetUserRole(username);

                if (userRole == null)
                {
                    return NotFound(username);
                }

                var vm = _mapper.Map<ReadedUserRoleViewModel>(userRole);
                return Accepted(vm);
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiAction(() =>
            {
                var users = _userService.GetAll();

                if (users == null)
                {
                    return NotFound();
                }

                var vm = users;
                return Accepted(vm);
            });
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }

    }
}