using System.Collections.Generic;
using ApiServer.Contracts.SkillType;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class SkillTypesControllerTest
    {
        private readonly SkillTypesController controller;
        private readonly Mock<ISkillTypeService> mockService;
        private readonly Mock<ILog<SkillTypesController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public SkillTypesControllerTest()
        {
            mockService = new Mock<ISkillTypeService>();
            mockLog = new Mock<ILog<SkillTypesController>>();
            mockMapper = new Mock<IMapper>();
            controller = new SkillTypesController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that get ActionResult")]
        public void GivenDataStored_WhenGet_ThenActionResult()
        {
            var expectedValue = new List<ReadedSkillTypeViewModel>();
            mockService.Setup(_ => _.List()).Returns(new List<ReadedSkillTypeContract>());
            mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<List<ReadedSkillTypeContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue, (result as AcceptedResult).Value);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<List<ReadedSkillTypeContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get NotFoundObjectResult when data is valid but cannot find ReadedSkillTypeContract")]
        public void GivenNoDataAndAnId_WhenGet_ThenNotFoundObjectResult()
        {
            int id = 0;

            var result = controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(id, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get ActionResult when data is valid and can find ReadedSkillTypeContract")]
        public void GivenDataAndAnId_WhenGet_ThenActionResult()
        {
            int id = 0;
            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillTypeContract());

            var result = controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that post with CreateSkillTypeViewModel returns CreatedResult when data is valid")]
        public void GivenData_WhenPost_ThenCreateSkillTypeViewModel()
        {
            var task = new CreateSkillTypeViewModel();
            mockService.Setup(_ => _.Create(It.IsAny<CreateSkillTypeContract>())).Returns(new CreatedSkillTypeContract());

            var result = controller.Post(task);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
            Assert.Equal("Get", (result as CreatedResult).Location);
            mockService.Verify(_ => _.Create(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreateSkillTypeContract>(It.IsAny<CreateSkillTypeViewModel>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillTypeViewModel>(It.IsAny<CreatedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that put with id and UpdateSkillTypeViewModel returns AcceptedResult when data is valid")]
        public void GivenDataAndAnId_WhenPut_ThenAcceptedResult()
        {
            int id = 0;
            var task = new UpdateSkillTypeViewModel();
            mockMapper.Setup(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>())).Returns(new UpdateSkillTypeContract());

            var result = controller.Put(id, task);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(new { id }.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete with id returns AcceptedResult when data is valid")]
        public void GivenAnId_WhenDelete_ThenAcceptedResult()
        {
            int id = 0;

            var result = controller.Delete(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that ping returns OkObjectResult")]
        public void GivenNothing_WhenPing_ThenOkObjectResult()
        {
            var result = controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}
