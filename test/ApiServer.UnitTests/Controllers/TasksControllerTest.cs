// <copyright file="TasksControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Task;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Task;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class TasksControllerTest
    {
        private readonly TasksController controller;
        private readonly Mock<ITaskService> mockTaskService;
        private readonly Mock<ILog<TasksController>> mockLog;
        private readonly Mock<IMapper> mockMapper;

        public TasksControllerTest()
        {
            this.mockTaskService = new Mock<ITaskService>();
            this.mockLog = new Mock<ILog<TasksController>>();
            this.mockMapper = new Mock<IMapper>();
            this.controller = new TasksController(this.mockTaskService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that get ActionResult")]
        public void Should_GetActionResult()
        {
            this.mockTaskService.Setup(_ => _.List()).Returns(new List<ReadedTaskContract>());
            this.mockMapper.Setup(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<ReadedTaskContract>())).Returns(new List<ReadedTaskViewModel>());

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockTaskService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<List<ReadedTaskContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get NotFoundObjectResult when data is valid but cannot find ReadedTaskContract")]
        public void Should_GetNotFoundObjectResult_When_DataIsValid()
        {
            int id = 0;

            var result = this.controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(id, (result as NotFoundObjectResult).Value);
            this.mockTaskService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get ActionResult when data is valid and can find ReadedTaskContract")]
        public void Should_GetActionResult_When_DataIsValid()
        {
            int id = 0;
            this.mockTaskService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedTaskContract());

            var result = this.controller.Get(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockTaskService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedTaskViewModel>(It.IsAny<ReadedTaskContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that post with CreateTaskViewModel returns CreatedResult when data is valid")]
        public void Should_PostCreateTaskViewModel_When_DataIsValid()
        {
            CreateTaskViewModel task = new CreateTaskViewModel();
            this.mockTaskService.Setup(_ => _.Create(It.IsAny<CreateTaskContract>())).Returns(new CreatedTaskContract());

            var result = this.controller.Post(task);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
            Assert.Equal("Get", (result as CreatedResult).Location);
            this.mockTaskService.Verify(_ => _.Create(It.IsAny<CreateTaskContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreateTaskContract>(It.IsAny<CreateTaskViewModel>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedTaskViewModel>(It.IsAny<CreatedTaskContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that put with id and UpdateTaskViewModel returns AcceptedResult when data is valid")]
        public void Should_PutAcceptedResult_When_DataIsValid()
        {
            int id = 0;
            UpdateTaskViewModel task = new UpdateTaskViewModel();
            this.mockMapper.Setup(_ => _.Map<UpdateTaskContract>(It.IsAny<UpdateTaskViewModel>())).Returns(new UpdateTaskContract());

            var result = this.controller.Put(id, task);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(new { id }.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockTaskService.Verify(_ => _.Update(It.IsAny<UpdateTaskContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateTaskContract>(It.IsAny<UpdateTaskViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete with id returns AcceptedResult when data is valid")]
        public void Should_DeleteAcceptedResult_When_DataIsValid()
        {
            int id = 0;

            var result = this.controller.Delete(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockTaskService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that ping returns OkObjectResult")]
        public void Should_PingOkObjectResult()
        {
            var result = this.controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }

        [Fact(DisplayName = "Verify that aprroves returns AcceptedResult when data is valid")]
        public void Should_ApproveAcceptedResult_When_DataIsValid()
        {
            int id = 0;

            var result = this.controller.Approve(id);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(new { id }.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockTaskService.Verify(_ => _.Approve(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetByUser returns AcceptedResult when data is valid")]
        public void Should_GetByUser_When_DataIsValid()
        {
            string consultantEmail = null;
            var expectedValue = new List<ReadedTaskViewModel>();
            this.mockTaskService.Setup(_ => _.ListByUser(It.IsAny<string>())).Returns(new List<ReadedTaskContract>());
            this.mockMapper.Setup(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<IEnumerable<ReadedTaskContract>>())).Returns(expectedValue);

            var result = this.controller.GetByUser(consultantEmail);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue, (result as AcceptedResult).Value);
            this.mockTaskService.Verify(_ => _.ListByUser(It.IsAny<string>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedTaskViewModel>>(It.IsAny<IEnumerable<ReadedTaskContract>>()), Times.Once);
        }
    }
}
