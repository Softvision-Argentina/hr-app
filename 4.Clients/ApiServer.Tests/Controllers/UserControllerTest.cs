using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Services.Interfaces.Services;
using Moq;

namespace ApiServer.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUserService> mockTaskService;
        private readonly Mock<ILog<UserController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public UserControllerTest()
        {
            mockTaskService = new Mock<IUserService>();
            mockLog = new Mock<ILog<UserController>>();
            mockMapper = new Mock<IMapper>();
            controller = new UserController(mockTaskService.Object, mockLog.Object, mockMapper.Object);
        }
    }
}
