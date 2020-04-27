using ApiServer.Contracts.Skills;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.Skill;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class SkillsControllerTest
    {
        private SkillsController controller;
        private Mock<ILog<SkillsController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ISkillService> mockService;

        public SkillsControllerTest()
        {
            mockLog = new Mock<ILog<SkillsController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ISkillService>();
            controller = new SkillsController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllSkills()
        {
            var expectedValue = new List<ReadedSkillViewModel>();
            expectedValue.Add(new ReadedSkillViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                Type = 0
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedSkillContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<IEnumerable<ReadedSkillContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedSkillViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<IEnumerable<ReadedSkillContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_SkillById_WhenDataIsValid()
        {
            var skillId = 0;
            var expectedValue = new ReadedSkillViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                Type = 0
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillContract());
            mockMapper.Setup(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>())).Returns(expectedValue);

            var result = controller.Get(skillId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedSkillViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_SkillById_WhenDataIsInvalid()
        {
            var skillId = 0;
            var expectedValue = skillId;

            var result = controller.Get(skillId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_Skill()
        {
            var skillVM = new CreateSkillViewModel
            {
                Name = "test",
                Description = "test",
                Type = 0
            };

            var expectedValue = new CreatedSkillViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateSkillContract>(It.IsAny<CreateSkillViewModel>())).Returns(new CreateSkillContract());
            mockMapper.Setup(_ => _.Map<CreatedSkillViewModel>(It.IsAny<CreatedSkillContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateSkillContract>())).Returns(new CreatedSkillContract());

            var result = controller.Post(skillVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedSkillViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateSkillContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillViewModel>(It.IsAny<CreatedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Skill()
        {
            var skillId = 0;
            var expectedValue = new { id = skillId };
            var skillToUpdate = new UpdateSkillViewModel();

            mockMapper.Setup(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>())).Returns(new UpdateSkillContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateSkillContract>()));

            var result = controller.Put(skillId, skillToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Skill()
        {
            var skillId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(skillId);

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
