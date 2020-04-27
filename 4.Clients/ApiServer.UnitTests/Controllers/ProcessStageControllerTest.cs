using ApiServer.Contracts.Stage;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class ProcessStageControllerTest
    {
        private ProcessStageController controller;
        private Mock<ILog<ProcessStageController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IProcessStageService> mockService;
        public ProcessStageControllerTest()
        {
            mockLog = new Mock<ILog<ProcessStageController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IProcessStageService>();
            controller = new ProcessStageController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllProcessStages()
        {
            var expectedValue = new List<ReadedStageViewModel>();
            expectedValue.Add(new ReadedStageViewModel
            {
                Id = 0,
                ProcessId = 0,
                Date = DateTime.Now,
                Status = StageStatus.InProgress,
                Feedback = "test",
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                RejectionReason = ""
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedStageContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedStageViewModel>>(It.IsAny<IEnumerable<ReadedStageContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedStageViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedStageViewModel>>(It.IsAny<IEnumerable<ReadedStageContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_ProcessStageById()
        {
            var processStageId = 0;
            var expectedValue = new ReadedStageViewModel
            {
                Id = 0,
                ProcessId = 0,
                Date = DateTime.Now,
                Status = StageStatus.InProgress,
                Feedback = "test",
                UserOwnerId = 0,
                UserOwner = null,
                UserDelegateId = 0,
                UserDelegate = null,
                RejectionReason = ""
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedStageContract());
            mockMapper.Setup(_ => _.Map<ReadedStageViewModel>(It.IsAny<ReadedStageContract>())).Returns(expectedValue);

            var result = controller.Get(processStageId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedStageViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedStageViewModel>(It.IsAny<ReadedStageContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_ProcessStage()
        {
            var processStageVM = new CreateStageViewModel
            {
                Id = 0,
                ProcessId = 0,
                Date = DateTime.Now,
                Status = StageStatus.InProgress,
                Feedback = "test",
                UserOwnerId = 0,
                UserDelegateId = 0,
                RejectionReason = ""
            };

            var expectedValue = new CreatedStageViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateStageContract>(It.IsAny<CreateStageViewModel>())).Returns(new CreateStageContract());
            mockMapper.Setup(_ => _.Map<CreatedStageViewModel>(It.IsAny<CreatedStageContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateStageContract>())).Returns(new CreatedStageContract());

            var result = controller.Post(processStageVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedStageViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateStageContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedStageViewModel>(It.IsAny<CreatedStageContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_ProcessStage()
        {
            var processStageId = 0;
            var processStageToUpdate = new UpdateStageViewModel();

            mockMapper.Setup(_ => _.Map<UpdateStageContract>(It.IsAny<UpdateStageViewModel>())).Returns(new UpdateStageContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateStageContract>()));

            var result = controller.Put(processStageId, processStageToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Update(It.IsAny<UpdateStageContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateStageContract>(It.IsAny<UpdateStageViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_ProcessStage()
        {
            var processStageId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(processStageId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}