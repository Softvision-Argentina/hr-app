// <copyright file="ReferralsControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.CandidateSkill;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ReferralsControllerTest
    {
        private readonly ReferralsController controller;
        private readonly Mock<ILog<ReferralsController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ICandidateService> mockService;

        public ReferralsControllerTest()
        {
            this.mockLog = new Mock<ILog<ReferralsController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<ICandidateService>();
            this.controller = new ReferralsController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_Referral()
        {
            var referralVM = new CreateCandidateViewModel
            {
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                EmailAddress = "jdoe@test.com",
                PhoneNumber = "22222222",
                LinkedInProfile = "testprofile",
                EnglishLevel = EnglishLevel.Advanced,
                Status = CandidateStatus.New,
                User = null,
                Community = null,
                Profile = null,
                IsReferred = false,
                ContactDay = DateTime.Now,
                CandidateSkills = new List<CreateCandidateSkillViewModel>(),
                Cv = string.Empty,
                KnownFrom = string.Empty,
                ReferredBy = string.Empty,
            };

            var expectedValue = new CreatedCandidateViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateCandidateContract>(It.IsAny<CreateCandidateViewModel>())).Returns(new CreateCandidateContract());
            this.mockMapper.Setup(_ => _.Map<CreatedCandidateViewModel>(It.IsAny<CreatedCandidateContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateCandidateContract>())).Returns(new CreatedCandidateContract());

            var result = this.controller.Post(referralVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCandidateViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateCandidateContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedCandidateViewModel>(It.IsAny<CreatedCandidateContract>()), Times.Once);
        }
    }
}
