using ApiServer.Contracts.Postulant;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Services.Contracts.Postulant;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class PostulantsControllerTest
    {
        private PostulantsController controller;
        private Mock<ILog<PostulantsController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IPostulantService> mockService;

        public PostulantsControllerTest()
        {
            mockLog = new Mock<ILog<PostulantsController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IPostulantService>();
            controller = new PostulantsController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllPostulants()
        {
            var expectedValue = new List<ReadedPostulantViewModel>();
            expectedValue.Add(new ReadedPostulantViewModel
            {
                Id = "0",
                Name = "John Doe",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                Cv = "",
                CreatedDate = DateTime.Now
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedPostulantContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedPostulantViewModel>>(It.IsAny<IEnumerable<ReadedPostulantContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedPostulantViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedPostulantViewModel>>(It.IsAny<IEnumerable<ReadedPostulantContract>>()), Times.Once);
        }
    }
}