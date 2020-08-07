// <copyright file="UserControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using ApiServer.Contracts.User;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.User;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUserService> mockService;
        private readonly Mock<ILog<UserController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public UserControllerTest()
        {
            this.mockService = new Mock<IUserService>();
            this.mockLog = new Mock<ILog<UserController>>();
            this.mockMapper = new Mock<IMapper>();
            this.controller = new UserController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that gets role by user name returns ActionResult when data is valid")]
        public void Should_GetRoleByUserNameActionResult_When_DataIsValid()
        {
            string username = "testUserName";
            var expectedValue = new ReadedUserRoleViewModel();
            this.mockService.Setup(_ => _.GetUserRole(It.IsAny<string>())).Returns(new ReadedUserRoleContract());
            this.mockMapper.Setup(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>())).Returns(expectedValue);

            var result = this.controller.GetRoleByUserName(username);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue, (result as AcceptedResult).Value);
            this.mockService.Verify(_ => _.GetUserRole(It.IsAny<string>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that gets role by user name returns NotFoundObjectResult when data is invalid")]
        public void Should_GetRoleByUserNameNotFoundObjectResult_When_DataIsInvalid()
        {
            string username = "testUserName";

            var result = this.controller.GetRoleByUserName(username);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(username, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.GetUserRole(It.IsAny<string>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedUserRoleViewModel>(It.IsAny<ReadedUserRoleContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that ping returns OkObjectResult")]
        public void Should_PingOkObjectResult()
        {
            var result = this.controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}
