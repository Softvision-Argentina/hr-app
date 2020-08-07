// <copyright file="EmployeeCasualtyControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.EmployeeCasualty;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.EmployeeCasualty;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class EmployeeCasualtyControllerTest
    {
        private readonly EmployeeCasualtyController controller;
        private readonly Mock<ILog<EmployeeCasualtyController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IEmployeeCasualtyService> mockService;

        public EmployeeCasualtyControllerTest()
        {
            this.mockLog = new Mock<ILog<EmployeeCasualtyController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IEmployeeCasualtyService>();
            this.controller = new EmployeeCasualtyController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllEmployeeCasualties()
        {
            var expectedValue = new List<ReadedEmployeeCasualtyViewModel>();
            expectedValue.Add(new ReadedEmployeeCasualtyViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedEmployeeCasualtyContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedEmployeeCasualtyViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeCasualtyContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedEmployeeCasualtyViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedEmployeeCasualtyViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeCasualtyContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_EmployeeCasualtyById_WhenDataIsValid()
        {
            var employeeCasualtyId = 0;
            var expectedValue = new ReadedEmployeeCasualtyViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedEmployeeCasualtyContract());
            this.mockMapper.Setup(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>())).Returns(expectedValue);

            var result = this.controller.Get(employeeCasualtyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeCasualtyViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_EmployeeCasualtyById_WhenDataIsInvalid()
        {
            var employeeCasualtyId = 0;
            var expectedValue = employeeCasualtyId;

            var result = this.controller.Get(employeeCasualtyId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_EmployeeCasualty()
        {
            var employeeCasualtyVM = new CreateEmployeeCasualtyViewModel
            {
                Month = 1,
                Year = 2020,
                Value = 500,
            };
            var expectedValue = new CreatedEmployeeCasualtyViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateEmployeeCasualtyContract>(It.IsAny<CreateEmployeeCasualtyViewModel>())).Returns(new CreateEmployeeCasualtyContract());
            this.mockMapper.Setup(_ => _.Map<CreatedEmployeeCasualtyViewModel>(It.IsAny<CreatedEmployeeCasualtyContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateEmployeeCasualtyContract>())).Returns(new CreatedEmployeeCasualtyContract());

            var result = this.controller.Post(employeeCasualtyVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedEmployeeCasualtyViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedEmployeeCasualtyViewModel>(It.IsAny<CreatedEmployeeCasualtyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_EmployeeCasualty()
        {
            var employeeCasualtyId = 0;
            var employeeCasualtyToUpdate = new UpdateEmployeeCasualtyViewModel();
            var expectedValue = new { id = employeeCasualtyId };

            this.mockMapper.Setup(_ => _.Map<UpdateEmployeeCasualtyContract>(It.IsAny<UpdateEmployeeCasualtyViewModel>())).Returns(new UpdateEmployeeCasualtyContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateEmployeeCasualtyContract>()));

            var result = this.controller.Put(employeeCasualtyId, employeeCasualtyToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateEmployeeCasualtyContract>(It.IsAny<UpdateEmployeeCasualtyViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_EmployeeCasualty()
        {
            var employeeCasualtyId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(employeeCasualtyId);

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