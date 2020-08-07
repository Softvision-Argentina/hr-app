// <copyright file="DeclineReasonControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class DeclineReasonControllerTest
    {
        private readonly DeclineReasonController controller;
        private readonly Mock<ILog<DeclineReasonController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IDeclineReasonService> mockService;

        public DeclineReasonControllerTest()
        {
            this.mockLog = new Mock<ILog<DeclineReasonController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IDeclineReasonService>();
            this.controller = new DeclineReasonController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDeclineReasons()
        {
            var expectedValue = new List<ReadedDeclineReasonViewModel>();
            expectedValue.Add(new ReadedDeclineReasonViewModel
            {
                Id = 0,
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDeclineReasonContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDeclineReasonViewModel>>(It.IsAny<IEnumerable<ReadedDeclineReasonContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDeclineReasonViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonViewModel>>(It.IsAny<IEnumerable<ReadedDeclineReasonContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetNamed' (which does not receive any data) returns AcceptedResult")]
        public void Should_GetNames_DeclineReasons()
        {
            var expectedValue = new List<ReadedDeclineReasonViewModel>();
            expectedValue.Add(new ReadedDeclineReasonViewModel
            {
                Id = 0,
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
            });

            this.mockService.Setup(_ => _.ListNamed()).Returns(new[] { new ReadedDeclineReasonContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDeclineReasonViewModel>>(It.IsAny<IEnumerable<ReadedDeclineReasonContract>>())).Returns(expectedValue);

            var result = this.controller.GetNamed();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDeclineReasonViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.ListNamed(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonViewModel>>(It.IsAny<IEnumerable<ReadedDeclineReasonContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DeclineReasonById_WhenDataIsValid()
        {
            var declineReasonId = 0;
            var expectedValue = new ReadedDeclineReasonViewModel
            {
                Id = 0,
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedDeclineReasonContract());
            this.mockMapper.Setup(_ => _.Map<ReadedDeclineReasonViewModel>(It.IsAny<ReadedDeclineReasonContract>())).Returns(expectedValue);

            var result = this.controller.Get(declineReasonId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDeclineReasonViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDeclineReasonViewModel>(It.IsAny<ReadedDeclineReasonContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DeclineReasonById_WhenDataIsInvalid()
        {
            var declineReasonId = 0;
            var expectedValue = declineReasonId;

            var result = this.controller.Get(declineReasonId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDeclineReasonViewModel>(It.IsAny<ReadedDeclineReasonContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_DeclineReason()
        {
            var declineReasonVM = new CreateDeclineReasonViewModel
            {
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
            };
            var expectedValue = new CreatedDeclineReasonViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateDeclineReasonContract>(It.IsAny<CreateDeclineReasonViewModel>())).Returns(new CreateDeclineReasonContract());
            this.mockMapper.Setup(_ => _.Map<CreatedDeclineReasonViewModel>(It.IsAny<CreatedDeclineReasonContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateDeclineReasonContract>())).Returns(new CreatedDeclineReasonContract());

            var result = this.controller.Post(declineReasonVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDeclineReasonViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateDeclineReasonContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedDeclineReasonViewModel>(It.IsAny<CreatedDeclineReasonContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_DeclineReason()
        {
            var declineReasonId = 0;
            var declineReasonToUpdate = new UpdateDeclineReasonViewModel();
            var expectedValue = new { id = declineReasonId };

            this.mockMapper.Setup(_ => _.Map<UpdateDeclineReasonContract>(It.IsAny<UpdateDeclineReasonViewModel>())).Returns(new UpdateDeclineReasonContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateDeclineReasonContract>()));

            var result = this.controller.Put(declineReasonId, declineReasonToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateDeclineReasonContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateDeclineReasonContract>(It.IsAny<UpdateDeclineReasonViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_DeclineReason()
        {
            var declineReasonId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(declineReasonId);

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