// <copyright file="CandidateProfileControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class CandidateProfileControllertTest
    {
        private readonly CandidateProfileController controller;
        private readonly Mock<ILog<CandidateProfileController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ICandidateProfileService> mockService;

        public CandidateProfileControllertTest()
        {
            this.mockLog = new Mock<ILog<CandidateProfileController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<ICandidateProfileService>();
            this.controller = new CandidateProfileController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllCandidateProfiles()
        {
            var expectedValue = new[]
            {
                new ReadedCandidateProfileViewModel
                {
                    Id = 0,
                    Name = "test",
                    Description = "test",
                    CommunityItems = null,
                },
            }
            .ToList();

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedCandidateProfileContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedCandidateProfileViewModel>>(It.IsAny<IEnumerable<ReadedCandidateProfileContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCandidateProfileViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCandidateProfileViewModel>>(It.IsAny<IEnumerable<ReadedCandidateProfileContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an 'id') returns AcceptedResult when data is valid")]
        public void Should_Get_CandidateProfilesById_WhenDataIsValid()
        {
            var candidateProfileId = 0;
            var expectedValue = new ReadedCandidateProfileViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                CommunityItems = null,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedCandidateProfileContract());
            this.mockMapper.Setup(_ => _.Map<ReadedCandidateProfileViewModel>(It.IsAny<ReadedCandidateProfileContract>())).Returns(expectedValue);

            var result = this.controller.Get(candidateProfileId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCandidateProfileViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);

            this.mockMapper.Verify(_ => _.Map<ReadedCandidateProfileViewModel>(It.IsAny<ReadedCandidateProfileContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an 'id') returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_CandidateProfilesById_WhenDataIsInvalid()
        {
            var candidateProfileId = 0;
            var expectedValue = candidateProfileId;

            var result = this.controller.Get(candidateProfileId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCandidateProfileViewModel>(It.IsAny<ReadedCandidateProfileContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_CandidateProfile()
        {
            var candidateProfileVM = new CreateCandidateProfileViewModel
            {
                Name = "test",
                Description = "test",
                CommunityItems = null,
            };

            var expectedValue = new CreatedCandidateProfileViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateCandidateProfileContract>(It.IsAny<CreateCandidateProfileViewModel>())).Returns(new CreateCandidateProfileContract());
            this.mockMapper.Setup(_ => _.Map<CreatedCandidateProfileViewModel>(It.IsAny<CreatedCandidateProfileContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateCandidateProfileContract>())).Returns(new CreatedCandidateProfileContract());

            var result = this.controller.Post(candidateProfileVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCandidateProfileViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateCandidateProfileContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedCandidateProfileViewModel>(It.IsAny<CreatedCandidateProfileContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_CandidateProfile()
        {
            var candidateId = 0;
            var expectedValue = new { id = candidateId };

            var candidateProfileToUpdate = new UpdateCandidateProfileViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateCandidateProfileContract>(It.IsAny<UpdateCandidateProfileViewModel>())).Returns(new UpdateCandidateProfileContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateCandidateProfileContract>()));

            var result = this.controller.Put(candidateId, candidateProfileToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateCandidateProfileContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateCandidateProfileContract>(It.IsAny<UpdateCandidateProfileViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_CandidateProfile()
        {
            var candidateId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(candidateId);

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