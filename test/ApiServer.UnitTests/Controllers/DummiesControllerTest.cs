// <copyright file="DummiesControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Seed;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Seed;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class DummiesControllerTest
    {
        private readonly DummiesController controller;
        private readonly Mock<ILog<DummiesController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IDummyService> mockService;

        public DummiesControllerTest()
        {
            this.mockLog = new Mock<ILog<DummiesController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IDummyService>();
            this.controller = new DummiesController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDummies()
        {
            var expectedValue = new List<ReadedDummyViewModel>();
            expectedValue.Add(new ReadedDummyViewModel
            {
                Id = Guid.NewGuid(),
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
                TestValue = string.Empty,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDummyContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDummyViewModel>>(It.IsAny<IEnumerable<ReadedDummyContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDummyViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDummyViewModel>>(It.IsAny<IEnumerable<ReadedDummyContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DummyById_WhenDataIsValid()
        {
            var dummyId = Guid.NewGuid();
            var expectedValue = new ReadedDummyViewModel
            {
                Id = dummyId,
                Name = "Test",
                Description = "Test",
                TestValue = string.Empty,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<Guid>())).Returns(new ReadedDummyContract());
            this.mockMapper.Setup(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>())).Returns(expectedValue);

            var result = this.controller.Get(dummyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDummyViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<Guid>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DummyById_WhenDataIsInvalid()
        {
            var dummyId = Guid.NewGuid();
            var expectedValue = dummyId;

            var result = this.controller.Get(dummyId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<Guid>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Dummy()
        {
            var dummyVM = new CreateDummyViewModel
            {
                Name = "Test",
                Description = "Test",
                TestValue = string.Empty,
            };
            var expectedValue = new CreatedDummyViewModel
            {
                Id = Guid.NewGuid(),
            };

            this.mockMapper.Setup(_ => _.Map<CreateDummyContract>(It.IsAny<CreateDummyViewModel>())).Returns(new CreateDummyContract());
            this.mockMapper.Setup(_ => _.Map<CreatedDummyViewModel>(It.IsAny<CreatedDummyContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateDummyContract>())).Returns(new CreatedDummyContract());

            var result = this.controller.Post(dummyVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDummyViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateDummyContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedDummyViewModel>(It.IsAny<CreatedDummyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Dummy()
        {
            var dummyId = Guid.NewGuid();
            var dummyToUpdate = new UpdateDummyViewModel();
            var expectedStatusCode = 202;

            this.mockMapper.Setup(_ => _.Map<UpdateDummyContract>(It.IsAny<UpdateDummyViewModel>())).Returns(new UpdateDummyContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateDummyContract>()));

            var result = this.controller.Put(dummyId, dummyToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedStatusCode, (result as AcceptedResult).StatusCode);
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateDummyContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateDummyContract>(It.IsAny<UpdateDummyViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Dummy()
        {
            var dummyId = Guid.NewGuid();

            this.mockService.Setup(_ => _.Delete(It.IsAny<Guid>()));

            var result = this.controller.Delete(dummyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Ping' returns OkObjectResult")]
        public void Should_Ping()
        {
            var result = this.controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}
