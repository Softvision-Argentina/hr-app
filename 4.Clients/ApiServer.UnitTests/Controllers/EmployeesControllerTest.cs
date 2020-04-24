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
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class EmployeesControllerTest
    {
        private EmployeesController controller;
        private Mock<ILog<EmployeesController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IEmployeeService> mockService;

        public EmployeesControllerTest()
        {
            mockLog = new Mock<ILog<EmployeesController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IEmployeeService>();
            controller = new EmployeesController(mockService.Object, mockLog.Object, mockMapper.Object);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedEmployeeContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedEmployeeViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeContract>>())).Returns(expectedValue);

            var result = controller.GetAll();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedEmployeeViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedEmployeeViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeContract>>()), Times.Once);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            };

            mockService.Setup(_ => _.GetById(It.IsAny<int>())).Returns(new Employee());
            mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = controller.GetById(employeeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.GetById(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            };

            mockService.Setup(_ => _.GetByDNI(It.IsAny<int>())).Returns(new Employee());
            mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = controller.GetByDNI(employeeIdentificationNumber);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.GetByDNI(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            };

            mockService.Setup(_ => _.GetByEmail(It.IsAny<string>())).Returns(new Employee());
            mockMapper.Setup(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>())).Returns(expectedValue);

            var result = controller.GetByEmail(employeeEmail);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.GetByEmail(It.IsAny<string>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeViewModel>(It.IsAny<Employee>()), Times.Once);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            };

            var expectedValue = new CreatedEmployeeViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateEmployeeContract>(It.IsAny<CreateEmployeeViewModel>())).Returns(new CreateEmployeeContract());
            mockMapper.Setup(_ => _.Map<CreatedEmployeeViewModel>(It.IsAny<CreatedEmployeeContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateEmployeeContract>())).Returns(new CreatedEmployeeContract());

            var result = controller.Add(employeeVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedEmployeeViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateEmployeeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedEmployeeViewModel>(It.IsAny<CreatedEmployeeContract>()), Times.Once);
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
                AdditionalInformation = "",
                Status = EmployeeStatus.Hired,
                UserId = 0,
                Role = null,
                isReviewer = false,
                Reviewer = null
            };            

            mockMapper.Setup(_ => _.Map<UpdateEmployeeContract>(It.IsAny<UpdateEmployeeViewModel>())).Returns(new UpdateEmployeeContract());
            mockService.Setup(_ => _.UpdateEmployee(It.IsAny<UpdateEmployeeContract>()));

            var result = controller.Update(employeeToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedStatusCode, (result as AcceptedResult).StatusCode);
            mockService.Verify(_ => _.UpdateEmployee(It.IsAny<UpdateEmployeeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateEmployeeContract>(It.IsAny<UpdateEmployeeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Employee()
        {
            var employeeId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(employeeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}