using ApiServer.Contracts.User;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiServer.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUserService> mockService;
        private readonly Mock<ILog<UserController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public UserControllerTest()
        {
            mockService = new Mock<IUserService>();
            mockLog = new Mock<ILog<UserController>>();
            mockMapper = new Mock<IMapper>();
            controller = new UserController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that gets role by user name returns ActionResult when data is valid")]
        public void Should_GetRoleByUserNameActionResult_When_DataIsValid()
        {
            string username = "testUserName";
            var expectedValue = new ReadedUserRoleViewModel();
            mockService.Setup(_ => _.GetUserRole(It.IsAny<string>())).Returns(new ReadedUserRoleContract());
            mockMapper.Setup(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>())).Returns(expectedValue);

            var result = controller.GetRoleByUserName(username);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue, (result as AcceptedResult).Value);
            mockService.Verify(_ => _.GetUserRole(It.IsAny<string>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that gets role by user name returns NotFoundObjectResult when data is invalid")]
        public void Should_GetRoleByUserNameNotFoundObjectResult_When_DataIsInvalid()
        {
            string username = "testUserName";

            var result = controller.GetRoleByUserName(username);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(username, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.GetUserRole(It.IsAny<string>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that ping returns OkObjectResult")]
        public void Should_PingOkObjectResult()
        {
            var result = controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}
