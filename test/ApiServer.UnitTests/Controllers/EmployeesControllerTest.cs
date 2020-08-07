// <copyright file="EmployeesControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Employee;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Employee;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class EmployeesControllerTest
    {
        private readonly EmployeesController controller;
        private readonly Mock<ILog<EmployeesController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IEmployeeService> mockService;

        public EmployeesControllerTest()
        {
            this.mockLog = new Mock<ILog<EmployeesController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IEmployeeService>();
            this.controller = new EmployeesController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'GetAll' (which does not receive any data) returns AcceptedResult")]
        public void Should_GetAll_Employees()
        {
            var expectedValue = new List<ReadedEmployeeViewModel>();
            expectedValue.Add(new ReadedEmployeeViewModel
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedEmployeeContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedEmployeeViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeContract>>())).Returns(expectedValue);

            var result = this.controller.GetAll();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedEmployeeViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedEmployeeViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetById' returns AcceptedResult")]
        public void Should_GetById()
        {
            var employeeId = 0;
            var expectedValue = new ReadedEmployeeViewModel
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            };

            this.mockService.Setup(_ => _.GetById(It.IsAny<int>())).Returns(new Employee());
            this.mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = this.controller.GetById(employeeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetByDNI' returns AcceptedResult")]
        public void Should_GetByDNI()
        {
            var employeeIdentificationNumber = 0;
            var expectedValue = new ReadedEmployeeViewModel
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            };

            this.mockService.Setup(_ => _.GetByDNI(It.IsAny<int>())).Returns(new Employee());
            this.mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = this.controller.GetByDNI(employeeIdentificationNumber);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.GetByDNI(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'GetByEmail' returns AcceptedResult")]
        public void Should_GetByEmail()
        {
            var employeeEmail = "jdoe@test.com";
            var expectedValue = new ReadedEmployeeViewModel
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            };

            this.mockService.Setup(_ => _.GetByEmail(It.IsAny<string>())).Returns(new Employee());
            this.mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = this.controller.GetByEmail(employeeEmail);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.GetByEmail(It.IsAny<string>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Add_Employee()
        {
            var employeeVM = new CreateEmployeeViewModel
            {
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            };

            var expectedValue = new CreatedEmployeeViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateEmployeeContract>(It.IsAny<CreateEmployeeViewModel>())).Returns(new CreateEmployeeContract());
            this.mockMapper.Setup(_ => _.Map<CreatedEmployeeViewModel>(It.IsAny<CreatedEmployeeContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateEmployeeContract>())).Returns(new CreatedEmployeeContract());

            var result = this.controller.Add(employeeVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedEmployeeViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateEmployeeContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedEmployeeViewModel>(It.IsAny<CreatedEmployeeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Update_Employee()
        {
            var expectedStatusCode = 202;
            var employeeToUpdate = new UpdateEmployeeViewModel()
            {
                Id = 0,
                Name = "John",
                LastName = "Doe",
                DNI = 11111111,
                PhoneNumber = "11111111",
                EmailAddress = "jdoe@test.com",
                LinkedInProfile = "testProfile",
                AdditionalInformation = string.Empty,
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                IsReviewer = false,
                Reviewer = null,
            };

            this.mockMapper.Setup(_ => _.Map<UpdateEmployeeContract>(It.IsAny<UpdateEmployeeViewModel>())).Returns(new UpdateEmployeeContract());
            this.mockService.Setup(_ => _.UpdateEmployee(It.IsAny<UpdateEmployeeContract>()));

            var result = this.controller.Update(employeeToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedStatusCode, (result as AcceptedResult).StatusCode);
            this.mockService.Verify(_ => _.UpdateEmployee(It.IsAny<UpdateEmployeeContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateEmployeeContract>(It.IsAny<UpdateEmployeeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Employee()
        {
            var employeeId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(employeeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}