// <copyright file="HireProjectionsControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.HireProjection;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.HireProjection;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class HireProjectionsControllerTest
    {
        private readonly HireProjectionsController controller;
        private readonly Mock<ILog<HireProjectionsController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IHireProjectionService> mockService;

        public HireProjectionsControllerTest()
        {
            this.mockLog = new Mock<ILog<HireProjectionsController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IHireProjectionService>();
            this.controller = new HireProjectionsController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllHireProjections()
        {
            var expectedValue = new List<ReadedHireProjectionViewModel>();
            expectedValue.Add(new ReadedHireProjectionViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedHireProjectionContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedHireProjectionViewModel>>(It.IsAny<IEnumerable<ReadedHireProjectionContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedHireProjectionViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedHireProjectionViewModel>>(It.IsAny<IEnumerable<ReadedHireProjectionContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_HireProjectionById_WhenDataIsValid()
        {
            var hireProjectionId = 0;
            var expectedValue = new ReadedHireProjectionViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedHireProjectionContract());
            this.mockMapper.Setup(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>())).Returns(expectedValue);

            var result = this.controller.Get(hireProjectionId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedHireProjectionViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_HireProjectionById_WhenDataIsInvalid()
        {
            var hireProjectionId = 0;
            var expectedValue = hireProjectionId;

            var result = this.controller.Get(hireProjectionId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_HireProjection()
        {
            var hireProjectionVM = new CreateHireProjectionViewModel
            {
                Month = 1,
                Year = 2020,
                Value = 500,
            };

            var expectedValue = new CreatedHireProjectionViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateHireProjectionContract>(It.IsAny<CreateHireProjectionViewModel>())).Returns(new CreateHireProjectionContract());
            this.mockMapper.Setup(_ => _.Map<CreatedHireProjectionViewModel>(It.IsAny<CreatedHireProjectionContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateHireProjectionContract>())).Returns(new CreatedHireProjectionContract());

            var result = this.controller.Post(hireProjectionVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedHireProjectionViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateHireProjectionContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedHireProjectionViewModel>(It.IsAny<CreatedHireProjectionContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_HireProjection()
        {
            var hireProjectionId = 0;
            var hireProjectionToUpdate = new UpdateHireProjectionViewModel();
            var expectedValue = new { id = hireProjectionId };

            this.mockMapper.Setup(_ => _.Map<UpdateHireProjectionContract>(It.IsAny<UpdateHireProjectionViewModel>())).Returns(new UpdateHireProjectionContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateHireProjectionContract>()));

            var result = this.controller.Put(hireProjectionId, hireProjectionToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateHireProjectionContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateHireProjectionContract>(It.IsAny<UpdateHireProjectionViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_HireProjection()
        {
            var hireProjectionId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(hireProjectionId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
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