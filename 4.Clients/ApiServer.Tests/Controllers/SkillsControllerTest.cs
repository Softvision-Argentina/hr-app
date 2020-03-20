using System.Collections.Generic;
using ApiServer.Contracts.Skills;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Skill;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiServer.Tests.Controllers
{
    public class SkillsControllerTest
    {
        private readonly SkillsController controller;
        private readonly Mock<ISkillService> mockService;
        private readonly Mock<ILog<SkillsController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public SkillsControllerTest()
        {
            mockService = new Mock<ISkillService>();
            mockLog = new Mock<ILog<SkillsController>>();
            mockMapper = new Mock<IMapper>();
            controller = new SkillsController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that get ActionResult")]
        public void Should_GetActionResult()
        {
            mockService.Setup(_ => _.List()).Returns(new List<ReadedSkillContract>());
            mockMapper.Setup(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<ReadedSkillContract>())).Returns(new List<ReadedSkillViewModel>());

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<List<ReadedSkillContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get NotFoundObjectResult when data is valid but cannot find ReadedSkillViewModel")]
        public void Should_GetNotFoundObjectResult_When_DataIsValid()
        {
            int id = 0;

            var result = controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(id, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get ActionResult when data is valid and can find ReadedSkillContract")]
        public void Should_GetActionResult_When_DataIsValid()
        {
            int id = 0;
            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillContract());

            var result = controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that post with CreatedSkillViewModel  returns CreatedResult when data is valid")]
        public void Should_PostCreatedSkillViewModel_When_DataIsValid()
        {
            var task = new CreateSkillViewModel();
            mockService.Setup(_ => _.Create(It.IsAny<CreateSkillContract>())).Returns(new CreatedSkillContract());

            var result = controller.Post(task);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
            Assert.Equal("Get", (result as CreatedResult).Location);
            mockService.Verify(_ => _.Create(It.IsAny<CreateSkillContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreateSkillContract>(It.IsAny<CreateSkillViewModel>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillViewModel>(It.IsAny<CreatedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that put with id and UpdateSkillViewModel returns AcceptedResult when data is valid")]
        public void Should_PutAcceptedResult_When_DataIsValid()
        {
            int id = 0;
            var task = new UpdateSkillViewModel();
            mockMapper.Setup(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>())).Returns(new UpdateSkillContract());

            var result = controller.Put(id, task);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(new { id }.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete with id returns AcceptedResult when data is valid")]
        public void Should_DeleteAcceptedResult_When_DataIsValid()
        {
            int id = 0;

            var result = controller.Delete(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
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
