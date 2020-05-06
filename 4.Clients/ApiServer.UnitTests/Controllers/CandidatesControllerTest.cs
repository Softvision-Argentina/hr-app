using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.CandidateSkill;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Model;
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
    public class CandidatesControllerTest
    {
        private CandidatesController controller;
        private Mock<ILog<CandidatesController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ICandidateService> mockService;
        public CandidatesControllerTest()
        {
            mockLog = new Mock<ILog<CandidatesController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ICandidateService>();
            controller = new CandidatesController(mockService.Object, mockLog.Object, mockMapper.Object);
        }


        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllCandidates()
        {
            var expectedValue = new List<ReadedCandidateViewModel>();
            expectedValue.Add(new ReadedCandidateViewModel
            {
                Id = 0,
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
                PreferredOfficeId = 1,
                CandidateSkills = new List<ReadedCandidateSkillViewModel>(),
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedCandidateContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedCandidateViewModel>>(It.IsAny<IEnumerable<ReadedCandidateContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCandidateViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCandidateViewModel>>(It.IsAny<IEnumerable<ReadedCandidateContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which filters candidates) returns AcceptedResult")]
        public void Should_Get_FilteredCandidates()
        {
            var filterData = new FilterCandidateViewModel();
            var expectedValue = new List<ReadedCandidateViewModel>();
            expectedValue.Add(new ReadedCandidateViewModel
            {
                Id = 0,
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
                PreferredOfficeId = 1,
                CandidateSkills = new List<ReadedCandidateSkillViewModel>(),
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            });

            mockService.Setup(_ => _.Read(It.IsAny<Func<Candidate, bool>>())).Returns(new[] { new ReadedCandidateContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedCandidateViewModel>>(It.IsAny<IEnumerable<ReadedCandidateContract>>())).Returns(expectedValue);

            var result = controller.Get(filterData);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCandidateViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<Func<Candidate, bool>>()), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCandidateViewModel>>(It.IsAny<IEnumerable<ReadedCandidateContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_CandidatesById_WhenDataIsValid()
        {
            var candidateId = 0;
            var expectedValue = new ReadedCandidateViewModel
            {
                Id = candidateId,
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
                PreferredOfficeId = 1,
                CandidateSkills = new List<ReadedCandidateSkillViewModel>(),
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedCandidateContract());
            mockMapper.Setup(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>())).Returns(expectedValue);

            var result = controller.Get(candidateId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCandidateViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_CandidatesById_WhenDataIsInvalid()
        {
            var candidateId = 0;
            var expectedValue = candidateId;

            var result = controller.Get(candidateId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Exists' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_VerifyIfCandidate_Exists_WhenDataIsValid()
        {
            var candidateId = 0;
            var expectedValue = new ReadedCandidateViewModel
            {
                Id = candidateId,
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
                PreferredOfficeId = 1,
                CandidateSkills = new List<ReadedCandidateSkillViewModel>(),
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            };

            mockService.Setup(_ => _.Exists(It.IsAny<int>())).Returns(new ReadedCandidateContract());
            mockMapper.Setup(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>())).Returns(expectedValue);

            var result = controller.Exists(candidateId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCandidateViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Exists(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Exists' (which receives an id) returns AcceptedResult when data is invalid")]
        public void Should_VerifyIfCandidate_Exists_WhenDataIsInvalid()
        {
            var candidateId = 0;
            var expectedValue = candidateId;

            var result = controller.Exists(candidateId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Exists(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCandidateViewModel>(It.IsAny<ReadedCandidateContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'GetCandidateApp' returns AcceptedResult")]
        public void Should_GetCandidateApp()
        {
            var expectedValue = new List<ReadedCandidateAppViewModel>();
            expectedValue.Add(new ReadedCandidateAppViewModel
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                EmailAddress = "test@test.com",
                PhoneNumber = "22222222",
                LinkedInProfile = "testprofile",                
                EnglishLevel = EnglishLevel.Advanced,
                Status = CandidateStatus.New,
                User = null,
                Community = null,
                Profile = null,
                IsReferred = false,
                ContactDay = DateTime.Now,
                PreferredOffice = null,
                CandidateSkills = new List<ReadedCandidateAppSkillViewModel>(),
                Cv = "",
                KnownFrom = "",
                ReferredBy = ""
            });

            mockService.Setup(_ => _.ListApp()).Returns(new[] { new ReadedCandidateAppContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedCandidateAppViewModel>>(It.IsAny<IEnumerable<ReadedCandidateAppContract>>())).Returns(expectedValue);

            var result = controller.GetCandidateApp();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCandidateAppViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.ListApp(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCandidateAppViewModel>>(It.IsAny<IEnumerable<ReadedCandidateAppContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Candidate()
        {
            var candidateVM = new CreateCandidateViewModel
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

            var result = controller.Post(candidateVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCandidateViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateCandidateContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedCandidateViewModel>(It.IsAny<CreatedCandidateContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Candidate()
        {
            var candidateId = 0;
            var candidateToUpdate = new UpdateCandidateViewModel();
            var expectedValue = new { id = candidateId};

            mockMapper.Setup(_ => _.Map<UpdateCandidateContract>(It.IsAny<UpdateCandidateViewModel>())).Returns(new UpdateCandidateContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateCandidateContract>()));

            var result = controller.Put(candidateId, candidateToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateCandidateContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateCandidateContract>(It.IsAny<UpdateCandidateViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Candidate()
        {
            var candidateId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(candidateId);

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
