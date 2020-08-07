// <copyright file="SkillTypesControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.SkillType;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.SkillType;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class SkillTypesControllerTest
    {
        private readonly SkillTypesController controller;
        private readonly Mock<ILog<SkillTypesController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ISkillTypeService> mockService;

        public SkillTypesControllerTest()
        {
            this.mockLog = new Mock<ILog<SkillTypesController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<ISkillTypeService>();
            this.controller = new SkillTypesController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllSkillTypes()
        {
            var expectedValue = new List<ReadedSkillTypeViewModel>();
            expectedValue.Add(new ReadedSkillTypeViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedSkillTypeContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<IEnumerable<ReadedSkillTypeContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedSkillTypeViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeViewModel>>(It.IsAny<IEnumerable<ReadedSkillTypeContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_SkillTypeById_WhenDataIsValid()
        {
            var skillTypeId = 0;
            var expectedValue = new ReadedSkillTypeViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedSkillTypeContract());
            this.mockMapper.Setup(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>())).Returns(expectedValue);

            var result = this.controller.Get(skillTypeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedSkillTypeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_SkillTypeById_WhenDataIsInvalid()
        {
            var skillTypeId = 0;
            var expectedValue = skillTypeId;

            var result = this.controller.Get(skillTypeId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillTypeViewModel>(It.IsAny<ReadedSkillTypeContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_SkillType()
        {
            var skillTypeVM = new CreateSkillTypeViewModel
            {
                Name = "test",
                Description = "test",
            };

            var expectedValue = new CreatedSkillTypeViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateSkillTypeContract>(It.IsAny<CreateSkillTypeViewModel>())).Returns(new CreateSkillTypeContract());
            this.mockMapper.Setup(_ => _.Map<CreatedSkillTypeViewModel>(It.IsAny<CreatedSkillTypeContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateSkillTypeContract>())).Returns(new CreatedSkillTypeContract());

            var result = this.controller.Post(skillTypeVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedSkillTypeViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillTypeViewModel>(It.IsAny<CreatedSkillTypeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_SkillType()
        {
            var skillTypeId = 0;
            var expectedValue = new { id = skillTypeId };
            var skillTypeToUpdate = new UpdateSkillTypeViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>())).Returns(new UpdateSkillTypeContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateSkillTypeContract>()));

            var result = this.controller.Put(skillTypeId, skillTypeToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateSkillTypeContract>(It.IsAny<UpdateSkillTypeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_SkillType()
        {
            var skillTypeId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(skillTypeId);

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