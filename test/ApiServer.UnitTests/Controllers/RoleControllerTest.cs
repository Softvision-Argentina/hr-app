// <copyright file="RoleControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Role;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Role;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class RoleControllerTest
    {
        private readonly RoleController controller;
        private readonly Mock<ILog<RoleController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRoleService> mockService;

        public RoleControllerTest()
        {
            this.mockLog = new Mock<ILog<RoleController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IRoleService>();
            this.controller = new RoleController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllRoles()
        {
            var expectedValue = new List<ReadedRoleViewModel>();
            expectedValue.Add(new ReadedRoleViewModel
            {
                Id = 0,
                Name = "test",
                IsActive = true,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedRoleContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedRoleViewModel>>(It.IsAny<IEnumerable<ReadedRoleContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedRoleViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedRoleViewModel>>(It.IsAny<IEnumerable<ReadedRoleContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_RoleById_WhenDataIsValid()
        {
            var roleId = 0;
            var expectedValue = new ReadedRoleViewModel
            {
                Id = 0,
                Name = "test",
                IsActive = true,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedRoleContract());
            this.mockMapper.Setup(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>())).Returns(expectedValue);

            var result = this.controller.Get(roleId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedRoleViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_RoleById_WhenDataIsInvalid()
        {
            var roleId = 0;
            var expectedValue = roleId;

            var result = this.controller.Get(roleId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Add_Role()
        {
            var roleVM = new CreateRoleViewModel
            {
                Name = "test",
                IsActive = true,
            };

            var expectedValue = new CreatedRoleViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateRoleContract>(It.IsAny<CreateRoleViewModel>())).Returns(new CreateRoleContract());
            this.mockMapper.Setup(_ => _.Map<CreatedRoleViewModel>(It.IsAny<CreatedRoleContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateRoleContract>())).Returns(new CreatedRoleContract());

            var result = this.controller.Add(roleVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedRoleViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateRoleContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedRoleViewModel>(It.IsAny<CreatedRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Update' returns AcceptedResult")]
        public void Should_Update_Role()
        {
            var roleId = 0;
            var expectedValue = new { id = roleId };
            var roleToUpdate = new UpdateRoleViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateRoleContract>(It.IsAny<UpdateRoleViewModel>())).Returns(new UpdateRoleContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateRoleContract>()));

            var result = this.controller.Update(roleId, roleToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateRoleContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateRoleContract>(It.IsAny<UpdateRoleViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Role()
        {
            var roleId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(roleId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
