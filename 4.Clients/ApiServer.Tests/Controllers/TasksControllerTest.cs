using System.Collections.Generic;
using ApiServer.Contracts.Task;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Task;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiServer.Tests.Controllers
{
    public class TasksControllerTest
    {
        private readonly TasksController controller;
        private readonly Mock<ITaskService> mockTaskService;
        private readonly Mock<ILog<TasksController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public TasksControllerTest()
        {
            mockTaskService = new Mock<ITaskService>();
            mockLog = new Mock<ILog<TasksController>>();
            mockMapper = new Mock<IMapper>();
            controller = new TasksController(mockTaskService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that get ActionResult")]
        public void Should_GetActionResult()
        {
            mockTaskService.Setup(_ => _.List()).Returns(new List<ReadedTaskContract>());
            mockMapper.Setup(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<ReadedTaskContract>())).Returns(new List<ReadedTaskViewModel>());

            var actionResult = controller.Get();

            Assert.NotNull(actionResult);
            Assert.IsType<AcceptedResult>(actionResult);
            mockTaskService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<List<ReadedTaskContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get NotFoundObjectResult when data is valid but cannot find ReadedTaskContract")]
        public void Should_GetNotFoundObjectResult_When_DataIsValid()
        {
            int id = 0;

            var actionResult = controller.Get(id);

            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal(id, (actionResult as NotFoundObjectResult).Value);
            mockTaskService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get ActionResult when data is valid and can find ReadedTaskContract")]
        public void Should_GetActionResult_When_DataIsValid()
        {
            int id = 0;
            mockTaskService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedTaskContract());

            var actionResult = controller.Get(id);

            Assert.NotNull(actionResult);
            Assert.IsType<AcceptedResult>(actionResult);
            mockTaskService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedTaskViewModel>(It.IsAny<ReadedTaskContract>()), Times.Once);
        }
    }
}
