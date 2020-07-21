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
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class EmployeeCasualtyControllerTest
    {
        private EmployeeCasualtyController controller;
        private Mock<ILog<EmployeeCasualtyController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IEmployeeCasualtyService> mockService;

        public EmployeeCasualtyControllerTest()
        {
            mockLog = new Mock<ILog<EmployeeCasualtyController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IEmployeeCasualtyService>();
            controller = new EmployeeCasualtyController(mockService.Object, mockLog.Object, mockMapper.Object);
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
                Value = 500
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedEmployeeCasualtyContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedEmployeeCasualtyViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeCasualtyContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedEmployeeCasualtyViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedEmployeeCasualtyViewModel>>(It.IsAny<IEnumerable<ReadedEmployeeCasualtyContract>>()), Times.Once);
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
                Value = 500
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedEmployeeCasualtyContract());
            mockMapper.Setup(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>())).Returns(expectedValue);

            var result = controller.Get(employeeCasualtyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedEmployeeCasualtyViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_EmployeeCasualtyById_WhenDataIsInvalid()
        {
            var employeeCasualtyId = 0;
            var expectedValue = employeeCasualtyId;

            var result = controller.Get(employeeCasualtyId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyViewModel>(It.IsAny<ReadedEmployeeCasualtyContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_EmployeeCasualty()
        {
            var employeeCasualtyVM = new CreateEmployeeCasualtyViewModel
            {
                Month = 1,
                Year = 2020,
                Value = 500
            };
            var expectedValue = new CreatedEmployeeCasualtyViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateEmployeeCasualtyContract>(It.IsAny<CreateEmployeeCasualtyViewModel>())).Returns(new CreateEmployeeCasualtyContract());
            mockMapper.Setup(_ => _.Map<CreatedEmployeeCasualtyViewModel>(It.IsAny<CreatedEmployeeCasualtyContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateEmployeeCasualtyContract>())).Returns(new CreatedEmployeeCasualtyContract());

            var result = controller.Post(employeeCasualtyVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedEmployeeCasualtyViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedEmployeeCasualtyViewModel>(It.IsAny<CreatedEmployeeCasualtyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_EmployeeCasualty()
        {
            var employeeCasualtyId = 0;
            var employeeCasualtyToUpdate = new UpdateEmployeeCasualtyViewModel();
            var expectedValue = new { id = employeeCasualtyId};

            mockMapper.Setup(_ => _.Map<UpdateEmployeeCasualtyContract>(It.IsAny<UpdateEmployeeCasualtyViewModel>())).Returns(new UpdateEmployeeCasualtyContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateEmployeeCasualtyContract>()));

            var result = controller.Put(employeeCasualtyId, employeeCasualtyToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateEmployeeCasualtyContract>(It.IsAny<UpdateEmployeeCasualtyViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_EmployeeCasualty()
        {
            var employeeCasualtyId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(employeeCasualtyId);

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