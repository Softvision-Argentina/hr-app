using ApiServer.Contracts.SkillType;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Model;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class SkillTypesControllerTest
    {
        private SkillTypesController controller;
        private Mock<ILog<SkillTypesController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ISkillTypeService> mockService;

        public SkillTypesControllerTest()
        {
            mockLog = new Mock<ILog<SkillTypesController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ISkillTypeService>();
            controller = new SkillTypesController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllSkillTypes()
        {
            var expectedValue = new List<ReadedSkillTypeViewModel>();
            expectedValue.Add(new ReadedSkillTypeViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test"
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedSkillTypeContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<IEnumerable<ReadedSkillTypeContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedSkillTypeViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<IEnumerable<ReadedSkillTypeContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_SkillTypeById_WhenDataIsValid()
        {
            var skillTypeId = 0;
            var expectedValue = new ReadedSkillTypeViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test"
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillTypeContract());
            mockMapper.Setup(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>())).Returns(expectedValue);

            var result = controller.Get(skillTypeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedSkillTypeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_SkillTypeById_WhenDataIsInvalid()
        {
            var skillTypeId = 0;
            var expectedValue = skillTypeId;

            var result = controller.Get(skillTypeId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_SkillType()
        {
            var skillTypeVM = new CreateSkillTypeViewModel
            {
                Name = "test",
                Description = "test"
            };

            var expectedValue = new CreatedSkillTypeViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateSkillTypeContract>(It.IsAny<CreateSkillTypeViewModel>())).Returns(new CreateSkillTypeContract());
            mockMapper.Setup(_ => _.Map<CreatedSkillTypeViewModel>(It.IsAny<CreatedSkillTypeContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateSkillTypeContract>())).Returns(new CreatedSkillTypeContract());

            var result = controller.Post(skillTypeVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedSkillTypeViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillTypeViewModel>(It.IsAny<CreatedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_SkillType()
        {
            var skillTypeId = 0;
            var expectedValue = new { id = skillTypeId };
            var skillTypeToUpdate = new UpdateSkillTypeViewModel();

            mockMapper.Setup(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>())).Returns(new UpdateSkillTypeContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateSkillTypeContract>()));

            var result = controller.Put(skillTypeId, skillTypeToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_SkillType()
        {
            var skillTypeId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(skillTypeId);

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