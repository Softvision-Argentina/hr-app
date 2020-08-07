// <copyright file="DaysOffControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApiServer.Contracts.DaysOff;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.DaysOff;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class DaysOffControllerTest
    {
        private readonly DaysOffController controller;
        private readonly Mock<ILog<DaysOffController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IDaysOffService> mockService;

        public DaysOffControllerTest()
        {
            this.mockLog = new Mock<ILog<DaysOffController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IDaysOffService>();
            this.controller = new DaysOffController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDaysOff()
        {
            var expectedValue = new List<ReadedDaysOffViewModel>();
            expectedValue.Add(new ReadedDaysOffViewModel
            {
                Id = 0,
                Status = DaysOffStatus.Accepted,
                Date = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DaysOffType.Holidays,
                EmployeeId = 0,
                Employee = null,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDaysOffContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDaysOffViewModel>>(It.IsAny<IEnumerable<ReadedDaysOffContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDaysOffViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDaysOffViewModel>>(It.IsAny<IEnumerable<ReadedDaysOffContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DaysOffById_WhenDataIsValid()
        {
            var dayOffId = 0;
            var expectedValue = new ReadedDaysOffViewModel
            {
                Id = 0,
                Status = DaysOffStatus.Accepted,
                Date = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DaysOffType.Holidays,
                EmployeeId = 0,
                Employee = null,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedDaysOffContract());
            this.mockMapper.Setup(_ => _.Map<ReadedDaysOffViewModel>(It.IsAny<ReadedDaysOffContract>())).Returns(expectedValue);

            var result = this.controller.Get(dayOffId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDaysOffViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDaysOffViewModel>(It.IsAny<ReadedDaysOffContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DaysOffById_WhenDataIsInvalid()
        {
            var dayOffId = 0;
            var expectedValue = dayOffId;

            var result = this.controller.Get(dayOffId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDaysOffViewModel>(It.IsAny<ReadedDaysOffContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'GetByDni' returns AcceptedResult when data is valid")]
        public void Should_GetByDni_WhenDataIsValid()
        {
            var userIdentificationNumber = 0;
            var expectedValueData = new ReadedDaysOffViewModel
            {
                Id = 0,
                Status = DaysOffStatus.Accepted,
                Date = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DaysOffType.Holidays,
                EmployeeId = 0,
                Employee = null,
            };
            var expectedValue = new[] { expectedValueData }.ToList();

            this.mockService.Setup(_ => _.ReadByDni(It.IsAny<int>())).Returns(new[] { new ReadedDaysOffContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDaysOffViewModel>>(It.IsAny<IEnumerable<ReadedDaysOffContract>>())).Returns(expectedValue);

            var result = this.controller.GetByDni(userIdentificationNumber);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDaysOffViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.ReadByDni(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDaysOffViewModel>>(It.IsAny<IEnumerable<ReadedDaysOffContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetByDni' returns NotFoundObjectResult when data is invalid")]
        public void Should_GetByDni_WhenDataIsInvalid()
        {
            var userIdentificationNumber = 0;
            var expectedValue = userIdentificationNumber;

            this.mockMapper.Setup(_ => _.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>())).Returns((List<ReadedDaysOffContract>)null);

            var result = this.controller.GetByDni(userIdentificationNumber);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.ReadByDni(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDaysOffViewModel>>(It.IsAny<IEnumerable<ReadedDaysOffContract>>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_DayOff()
        {
            var dayOffVM = new CreateDaysOffViewModel
            {
                Status = DaysOffStatus.Accepted,
                Date = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Type = DaysOffType.Holidays,
                EmployeeId = 0,
                Employee = null,
            };

            var expectedValue = new CreatedDaysOffViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateDaysOffContract>(It.IsAny<CreateDaysOffViewModel>())).Returns(new CreateDaysOffContract());
            this.mockMapper.Setup(_ => _.Map<CreatedDaysOffViewModel>(It.IsAny<CreatedDaysOffContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateDaysOffContract>())).Returns(new CreatedDaysOffContract());

            var result = this.controller.Post(dayOffVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDaysOffViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateDaysOffContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedDaysOffViewModel>(It.IsAny<CreatedDaysOffContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_DayOff()
        {
            var dayOffId = 0;
            var dayOffToUpdate = new UpdateDaysOffViewModel();
            var expectedValue = new { id = dayOffId };

            this.mockMapper.Setup(_ => _.Map<UpdateDaysOffContract>(It.IsAny<UpdateDaysOffViewModel>())).Returns(new UpdateDaysOffContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateDaysOffContract>()));

            var result = this.controller.Put(dayOffId, dayOffToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateDaysOffContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateDaysOffContract>(It.IsAny<UpdateDaysOffViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_DayOff()
        {
            var dayOffId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(dayOffId);

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