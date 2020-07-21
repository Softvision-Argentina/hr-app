using ApiServer.Contracts.Process;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Model.Enum;
using Domain.Services.Contracts.Process;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class ProcessControllerTest
    {
        private ProcessController controller;
        private Mock<ILog<ProcessController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IProcessService> mockService;

        public ProcessControllerTest()
        {
            mockLog = new Mock<ILog<ProcessController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IProcessService>();
            controller = new ProcessController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllProcesss()
        {
            var expectedValue = new List<ReadedProcessViewModel>();
            expectedValue.Add(new ReadedProcessViewModel
            {
                Id = 0,
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedProcessContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_ProcessById()
        {
            var processId = 0;
            var expectedValue = new ReadedProcessViewModel
            {
                Id = 0,
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedProcessContract());
            mockMapper.Setup(_ => _.Map<ReadedProcessViewModel>(It.IsAny<ReadedProcessContract>())).Returns(expectedValue);

            var result = controller.Get(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedProcessViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedProcessViewModel>(It.IsAny<ReadedProcessContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetProcessesByCommunity' returns AcceptedResult")]
        public void Should_GetProcessesByCommunity()
        {
            var communityName = "test";
            var expectedValue = new List<ReadedProcessViewModel>();
            expectedValue.Add(new ReadedProcessViewModel
            {
                Id = 0,
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            });

            mockService.Setup(_ => _.GetProcessesByCommunity(It.IsAny<string>()));
            mockMapper.Setup(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);

            var result = controller.GetProcessesByCommunity(communityName);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.GetProcessesByCommunity(It.IsAny<string>()), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Process()
        {
            var processVM = new CreateProcessViewModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Interviewer = null,
                UserOwnerId = 0,
                UserDelegateId = 0,
                DelegateName = "Magic",
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            };

            var expectedValue = new CreatedProcessViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateProcessContract>(It.IsAny<CreateProcessViewModel>())).Returns(new CreateProcessContract());
            mockMapper.Setup(_ => _.Map<CreatedProcessViewModel>(It.IsAny<CreatedProcessContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateProcessContract>())).Returns(new CreatedProcessContract());

            var result = controller.Post(processVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedProcessViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateProcessContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedProcessViewModel>(It.IsAny<CreatedProcessContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Process()
        {
            var processId = 0;
            var processToUpdate = new UpdateProcessViewModel();
            var expectedValue = new List<ReadedProcessViewModel>();
            expectedValue.Add(new ReadedProcessViewModel
            {
                Id = 0,
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            });

            mockMapper.Setup(_ => _.Map<UpdateProcessContract>(It.IsAny<UpdateProcessViewModel>())).Returns(new UpdateProcessContract());
            mockMapper.Setup(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);
            mockService.Setup(_ => _.Update(It.IsAny<UpdateProcessContract>()));
            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedProcessContract() });

            var result = controller.Put(processId, processToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Update(It.IsAny<UpdateProcessContract>()), Times.Once);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateProcessContract>(It.IsAny<UpdateProcessViewModel>()), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Process()
        {
            var processId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Approve' returns AcceptedResult")]
        public void Should_Approve_Process()
        {
            var processId = 0;

            mockService.Setup(_ => _.Approve(It.IsAny<int>()));

            var result = controller.Approve(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Approve(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Reject' returns AcceptedResult")]
        public void Should_Reject_Process()
        {
            var rejectedProcessVM = new RejectProcessViewModel
            {
                Id = 0,
                RejectionReason = "ThisIsATest"
            };

            mockService.Setup(_ => _.Reject(It.IsAny<int>(), It.IsAny<string>()));

            var result = controller.Reject(rejectedProcessVM);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetActiveProcessByCandidate' returns AcceptedResult")]
        public void Should_GetActiveProcessByCandidate()
        {
            var candidateId = 0;
            var expectedValue = new List<ReadedProcessViewModel>();
            expectedValue.Add(new ReadedProcessViewModel
            {
                Id = 0,
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Status = ProcessStatus.InProgress,
                CurrentStage = ProcessCurrentStage.HrStage,
                RejectionReason = "",
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                ActualSalary = 500,
                WantedSalary = 1000,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null
            });

            mockService.Setup(_ => _.GetActiveByCandidateId(It.IsAny<int>()));
            mockMapper.Setup(_ => _.Map<IEnumerable<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);
            var result = controller.GetActiveProcessByCandidate(candidateId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.GetActiveByCandidateId(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<IEnumerable<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
        }
    }
}