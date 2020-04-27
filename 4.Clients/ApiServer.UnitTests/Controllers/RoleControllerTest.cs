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
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class RoleControllerTest
    {
        private RoleController controller;
        private Mock<ILog<RoleController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IRoleService> mockService;

        public RoleControllerTest()
        {
            mockLog = new Mock<ILog<RoleController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IRoleService>();
            controller = new RoleController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllRoles()
        {
            var expectedValue = new List<ReadedRoleViewModel>();
            expectedValue.Add(new ReadedRoleViewModel
            {
                Id = 0,
                Name = "test",
                isActive = true
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedRoleContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedRoleViewModel>>(It.IsAny<IEnumerable<ReadedRoleContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedRoleViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedRoleViewModel>>(It.IsAny<IEnumerable<ReadedRoleContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_RoleById_WhenDataIsValid()
        {
            var roleId = 0;
            var expectedValue = new ReadedRoleViewModel
            {
                Id = 0,
                Name = "test",
                isActive = true
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedRoleContract());
            mockMapper.Setup(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>())).Returns(expectedValue);

            var result = controller.Get(roleId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedRoleViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_RoleById_WhenDataIsInvalid()
        {
            var roleId = 0;
            var expectedValue = roleId;

            var result = controller.Get(roleId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedRoleViewModel>(It.IsAny<ReadedRoleContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Add_Role()
        {
            var roleVM = new CreateRoleViewModel
            {
                Name = "test",
                isActive = true
            };

            var expectedValue = new CreatedRoleViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateRoleContract>(It.IsAny<CreateRoleViewModel>())).Returns(new CreateRoleContract());
            mockMapper.Setup(_ => _.Map<CreatedRoleViewModel>(It.IsAny<CreatedRoleContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateRoleContract>())).Returns(new CreatedRoleContract());

            var result = controller.Add(roleVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedRoleViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateRoleContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedRoleViewModel>(It.IsAny<CreatedRoleContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Update' returns AcceptedResult")]
        public void Should_Update_Role()
        {
            var roleId = 0;
            var expectedValue = (new { id = roleId });
            var roleToUpdate = new UpdateRoleViewModel();

            mockMapper.Setup(_ => _.Map<UpdateRoleContract>(It.IsAny<UpdateRoleViewModel>())).Returns(new UpdateRoleContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateRoleContract>()));

            var result = controller.Update(roleId, roleToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateRoleContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateRoleContract>(It.IsAny<UpdateRoleViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Role()
        {
            var roleId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(roleId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
