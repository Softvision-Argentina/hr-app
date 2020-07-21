using ApiServer.Contracts.CompanyCalendar;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.CompanyCalendar;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class CompanyCalendarControllerTest
    {
        private CompanyCalendarController controller;
        private Mock<ILog<CompanyCalendarController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<ICompanyCalendarService> mockService;

        public CompanyCalendarControllerTest()
        {
            mockLog = new Mock<ILog<CompanyCalendarController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<ICompanyCalendarService>();
            controller = new CompanyCalendarController(mockService.Object, mockLog.Object, mockMapper.Object);
        }


        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllCompanyCalendars()
        {
            var expectedValue = new List<ReadedCompanyCalendarViewModel>();
            expectedValue.Add(new ReadedCompanyCalendarViewModel
            {
                Id = 0,
                Type = "test",
                Date = DateTime.Now,
                Comments = "test"
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedCompanyCalendarContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedCompanyCalendarViewModel>>(It.IsAny<IEnumerable<ReadedCompanyCalendarContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedCompanyCalendarViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCompanyCalendarViewModel>>(It.IsAny<IEnumerable<ReadedCompanyCalendarContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_CompanyCalendarById_WhenDataIsValid()
        {
            var companyCalendarId = 0;
            var expectedValue = new ReadedCompanyCalendarViewModel
            {
                Id = 0,
                Type = "test",
                Date = DateTime.Now,
                Comments = "test"
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedCompanyCalendarContract());
            mockMapper.Setup(_ => _.Map<ReadedCompanyCalendarViewModel>(It.IsAny<ReadedCompanyCalendarContract>())).Returns(expectedValue);

            var result = controller.Get(companyCalendarId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedCompanyCalendarViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCompanyCalendarViewModel>(It.IsAny<ReadedCompanyCalendarContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_CompanyCalendarById_WhenDataIsInvalid()
        {
            var companyCalendarId = 0;
            var expectedValue = companyCalendarId;

            var result = controller.Get(companyCalendarId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCompanyCalendarViewModel>(It.IsAny<ReadedCompanyCalendarContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_CompanyCalendar()
        {
            var companyCalendarVM = new CreateCompanyCalendarViewModel
            {
                Type = "test",
                Date = DateTime.Now,
                Comments = "test"
            };

            var expectedValue = new CreatedCompanyCalendarViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateCompanyCalendarContract>(It.IsAny<CreateCompanyCalendarViewModel>())).Returns(new CreateCompanyCalendarContract());
            mockMapper.Setup(_ => _.Map<CreatedCompanyCalendarViewModel>(It.IsAny<CreatedCompanyCalendarContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateCompanyCalendarContract>())).Returns(new CreatedCompanyCalendarContract());

            var result = controller.Post(companyCalendarVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedCompanyCalendarViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateCompanyCalendarContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedCompanyCalendarViewModel>(It.IsAny<CreatedCompanyCalendarContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_CompanyCalendar()
        {
            var companyCalendarId = 0;
            var companyCalendarToUpdate = new UpdateCompanyCalendarViewModel();
            var expectedValue = new { id = companyCalendarId };

            mockMapper.Setup(_ => _.Map<UpdateCompanyCalendarContract>(It.IsAny<UpdateCompanyCalendarViewModel>())).Returns(new UpdateCompanyCalendarContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateCompanyCalendarContract>()));

            var result = controller.Put(companyCalendarId, companyCalendarToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateCompanyCalendarContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateCompanyCalendarContract>(It.IsAny<UpdateCompanyCalendarViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_CompanyCalendar()
        {
            var companyCalendarId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(companyCalendarId);

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
