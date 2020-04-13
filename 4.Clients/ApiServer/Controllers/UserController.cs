using ApiServer.Contracts.User;
using AutoMapper;
using Core;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            ILog<UserController> logger,
            IMapper mapper) : base(logger)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //Todo: convention over configuration
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

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}