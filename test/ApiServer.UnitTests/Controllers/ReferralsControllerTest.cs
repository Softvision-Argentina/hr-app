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
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class ReferralsControllerTest
    {
        private ReferralsController controller;
        private Mock<ILog<ReferralsController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ICandidateService> mockService;

        public ReferralsControllerTest()
        {
            mockLog = new Mock<ILog<ReferralsController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ICandidateService>();
            controller = new ReferralsController(mockService.Object, mockLog.Object, mockMapper.Object);
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
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            };

            var expectedValue = new CreatedCandidateViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateCandidateContract>(It.IsAny<CreateCandidateViewModel>())).Returns(new CreateCandidateContract());
            mockMapper.Setup(_ => _.Map<CreatedCandidateViewModel>(It.IsAny<CreatedCandidateContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateCandidateContract>())).Returns(new CreatedCandidateContract());

            var result = controller.Post(referralVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCandidateViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateCandidateContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedCandidateViewModel>(It.IsAny<CreatedCandidateContract>()), Times.Once);
        }
    }
}
