// <copyright file="ProcessControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Process;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Process;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ProcessControllerTest
    {
        private readonly ProcessController controller;
        private readonly Mock<ILog<ProcessController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IProcessService> mockService;

        public ProcessControllerTest()
        {
            this.mockLog = new Mock<ILog<ProcessController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IProcessService>();
            this.controller = new ProcessController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
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
                RejectionReason = string.Empty,
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedProcessContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
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
                RejectionReason = string.Empty,
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedProcessContract());
            this.mockMapper.Setup(_ => _.Map<ReadedProcessViewModel>(It.IsAny<ReadedProcessContract>())).Returns(expectedValue);

            var result = this.controller.Get(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedProcessViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedProcessViewModel>(It.IsAny<ReadedProcessContract>()), Times.Once);
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
                RejectionReason = string.Empty,
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null,
            });

            this.mockService.Setup(_ => _.GetProcessesByCommunity(It.IsAny<string>()));
            this.mockMapper.Setup(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);

            var result = this.controller.GetProcessesByCommunity(communityName);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.GetProcessesByCommunity(It.IsAny<string>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
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
                RejectionReason = string.Empty,
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
                OfferStage = null,
            };

            var expectedValue = new CreatedProcessViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateProcessContract>(It.IsAny<CreateProcessViewModel>())).Returns(new CreateProcessContract());
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateProcessContract>())).Returns(new Process());
            this.mockMapper.Setup(_ => _.Map<CreatedProcessViewModel>(It.IsAny<Process>())).Returns(expectedValue);

            var result = this.controller.Post(processVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedProcessViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateProcessContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedProcessViewModel>(It.IsAny<Process>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Process()
        {
            var processId = 0;
            var processToUpdate = new UpdateProcessViewModel();
            var expectedValue = new TableProcessViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateProcessContract>(It.IsAny<UpdateProcessViewModel>())).Returns(new UpdateProcessContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateProcessContract>())).Returns(new Process());
            this.mockMapper.Setup(_ => _.Map<TableProcessViewModel>(It.IsAny<Process>())).Returns(new TableProcessViewModel());

            var result = this.controller.Put(processId, processToUpdate);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as TableProcessViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateProcessContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateProcessContract>(It.IsAny<UpdateProcessViewModel>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<TableProcessViewModel>(It.IsAny<Process>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Process()
        {
            var processId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Approve' returns AcceptedResult")]
        public void Should_Approve_Process()
        {
            var processId = 0;

            this.mockService.Setup(_ => _.Approve(It.IsAny<int>()));

            var result = this.controller.Approve(processId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Approve(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Reject' returns AcceptedResult")]
        public void Should_Reject_Process()
        {
            var rejectedProcessVM = new RejectProcessViewModel
            {
                Id = 0,
                RejectionReason = "ThisIsATest",
            };

            this.mockService.Setup(_ => _.Reject(It.IsAny<int>(), It.IsAny<string>()));

            var result = this.controller.Reject(rejectedProcessVM);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
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
                RejectionReason = string.Empty,
                DeclineReason = null,
                CandidateId = 0,
                Candidate = null,
                Postulant = null,
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                EnglishLevel = EnglishLevel.Advanced,
                Seniority = Seniority.Senior3,
                HireDate = DateTime.Now.AddMonths(1),
                HrStage = null,
                TechnicalStage = null,
                ClientStage = null,
                OfferStage = null,
            });

            this.mockService.Setup(_ => _.GetActiveByCandidateId(It.IsAny<int>()));
            this.mockMapper.Setup(_ => _.Map<IEnumerable<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>())).Returns(expectedValue);
            var result = this.controller.GetActiveProcessByCandidate(candidateId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedProcessViewModel>;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.GetActiveByCandidateId(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<IEnumerable<ReadedProcessViewModel>>(It.IsAny<IEnumerable<ReadedProcessContract>>()), Times.Once);
        }
    }
}