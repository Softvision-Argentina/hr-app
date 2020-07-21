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
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class CommunityControllerTest
    {
        private CommunityController controller;
        private Mock<ILog<CommunityController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ICommunityService> mockService;

        public CommunityControllerTest()
        {
            mockLog = new Mock<ILog<CommunityController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ICommunityService>();
            controller = new CommunityController(mockService.Object, mockLog.Object, mockMapper.Object);
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
                Profile = null
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedCommunityContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedCommunityViewModel>>(It.IsAny<IEnumerable<ReadedCommunityContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCommunityViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCommunityViewModel>>(It.IsAny<IEnumerable<ReadedCommunityContract>>()), Times.Once);
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
                Profile = null
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedCommunityContract());
            mockMapper.Setup(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>())).Returns(expectedValue);

            var result = controller.Get(communityId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCommunityViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_CommunityById_WhenDataIsInvalid()
        {
            var communityId = 0;
            var expectedValue = communityId;

            var result = controller.Get(communityId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCommunityViewModel>(It.IsAny<ReadedCommunityContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Community()
        {
            var communityVM = new CreateCommunityViewModel
            {
                Name = "test",
                Description = "test",
                ProfileId = 0,
                Profile = null
            };

            var expectedValue = new CreatedCommunityViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateCommunityContract>(It.IsAny<CreateCommunityViewModel>())).Returns(new CreateCommunityContract());
            mockMapper.Setup(_ => _.Map<CreatedCommunityViewModel>(It.IsAny<CreatedCommunityContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateCommunityContract>())).Returns(new CreatedCommunityContract());

            var result = controller.Post(communityVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCommunityViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateCommunityContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedCommunityViewModel>(It.IsAny<CreatedCommunityContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Community()
        {
            var communityId = 0;
            var communityToUpdate = new UpdateCommunityViewModel();
            var expectedValue = new { id = communityId};

            mockMapper.Setup(_ => _.Map<UpdateCommunityContract>(It.IsAny<UpdateCommunityViewModel>())).Returns(new UpdateCommunityContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateCommunityContract>()));

            var result = controller.Put(communityId, communityToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateCommunityContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateCommunityContract>(It.IsAny<UpdateCommunityViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Community()
        {
            var communityId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(communityId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Ping' returns OkObjectResult")]
        public void Should_Ping()
        {
            var result = controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}