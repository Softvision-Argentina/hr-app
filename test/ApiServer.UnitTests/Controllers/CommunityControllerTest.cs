// <copyright file="CommunityControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Community;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class CommunityControllerTest
    {
        private readonly CommunityController controller;
        private readonly Mock<ILog<CommunityController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ICommunityService> mockService;

        public CommunityControllerTest()
        {
            this.mockLog = new Mock<ILog<CommunityController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<ICommunityService>();
            this.controller = new CommunityController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllCommunities()
        {
            var expectedValue = new List<ReadedCommunityViewModel>();
            expectedValue.Add(new ReadedCommunityViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                ProfileId = 0,
                Profile = null,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedCommunityContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedCommunityViewModel>>(It.IsAny<IEnumerable<ReadedCommunityContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCommunityViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCommunityViewModel>>(It.IsAny<IEnumerable<ReadedCommunityContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_CommunityById_WhenDataIsValid()
        {
            var communityId = 0;
            var expectedValue = new ReadedCommunityViewModel
            {
                Id = communityId,
                Name = "test",
                Description = "test",
                ProfileId = 0,
                Profile = null,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedCommunityContract());
            this.mockMapper.Setup(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>())).Returns(expectedValue);

            var result = this.controller.Get(communityId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCommunityViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_CommunityById_WhenDataIsInvalid()
        {
            var communityId = 0;
            var expectedValue = communityId;

            var result = this.controller.Get(communityId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Community()
        {
            var communityVM = new CreateCommunityViewModel
            {
                Name = "test",
                Description = "test",
                ProfileId = 0,
                Profile = null,
            };

            var expectedValue = new CreatedCommunityViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateCommunityContract>(It.IsAny<CreateCommunityViewModel>())).Returns(new CreateCommunityContract());
            this.mockMapper.Setup(_ => _.Map<CreatedCommunityViewModel>(It.IsAny<CreatedCommunityContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateCommunityContract>())).Returns(new CreatedCommunityContract());

            var result = this.controller.Post(communityVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCommunityViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateCommunityContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedCommunityViewModel>(It.IsAny<CreatedCommunityContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Community()
        {
            var communityId = 0;
            var communityToUpdate = new UpdateCommunityViewModel();
            var expectedValue = new { id = communityId };

            this.mockMapper.Setup(_ => _.Map<UpdateCommunityContract>(It.IsAny<UpdateCommunityViewModel>())).Returns(new UpdateCommunityContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateCommunityContract>()));

            var result = this.controller.Put(communityId, communityToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateCommunityContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateCommunityContract>(It.IsAny<UpdateCommunityViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Community()
        {
            var communityId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(communityId);

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