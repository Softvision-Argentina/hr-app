// <copyright file="SkillsControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
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
    using Xunit;

    public class SkillsControllerTest
    {
        private readonly SkillsController controller;
        private readonly Mock<ILog<SkillsController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ISkillService> mockService;

        public SkillsControllerTest()
        {
            this.mockLog = new Mock<ILog<SkillsController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<ISkillService>();
            this.controller = new SkillsController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
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
                Type = 0,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedSkillContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<IEnumerable<ReadedSkillContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedSkillViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedSkillViewModel>>(It.IsAny<IEnumerable<ReadedSkillContract>>()), Times.Once);
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
                Type = 0,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillContract());
            this.mockMapper.Setup(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>())).Returns(expectedValue);

            var result = this.controller.Get(skillId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedSkillViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_SkillById_WhenDataIsInvalid()
        {
            var skillId = 0;
            var expectedValue = skillId;

            var result = this.controller.Get(skillId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillViewModel>(It.IsAny<ReadedSkillContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_Skill()
        {
            var skillVM = new CreateSkillViewModel
            {
                Name = "test",
                Description = "test",
                Type = 0,
            };

            var expectedValue = new CreatedSkillViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateSkillContract>(It.IsAny<CreateSkillViewModel>())).Returns(new CreateSkillContract());
            this.mockMapper.Setup(_ => _.Map<CreatedSkillViewModel>(It.IsAny<CreatedSkillContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateSkillContract>())).Returns(new CreatedSkillContract());

            var result = this.controller.Post(skillVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedSkillViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateSkillContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillViewModel>(It.IsAny<CreatedSkillContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Skill()
        {
            var skillId = 0;
            var expectedValue = new { id = skillId };
            var skillToUpdate = new UpdateSkillViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>())).Returns(new UpdateSkillContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateSkillContract>()));

            var result = this.controller.Put(skillId, skillToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateSkillContract>(It.IsAny<UpdateSkillViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Skill()
        {
            var skillId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(skillId);

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
