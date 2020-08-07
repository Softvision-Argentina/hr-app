// <copyright file="PostulantsControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Postulant;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Domain.Services.Contracts.Postulant;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class PostulantsControllerTest
    {
        private readonly PostulantsController controller;
        private readonly Mock<ILog<PostulantsController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IPostulantService> mockService;

        public PostulantsControllerTest()
        {
            this.mockLog = new Mock<ILog<PostulantsController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IPostulantService>();
            this.controller = new PostulantsController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
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
                Cv = string.Empty,
                CreatedDate = DateTime.Now,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedPostulantContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedPostulantViewModel>>(It.IsAny<IEnumerable<ReadedPostulantContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedPostulantViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedPostulantViewModel>>(It.IsAny<IEnumerable<ReadedPostulantContract>>()), Times.Once);
        }
    }
}