using ApiServer.Contracts.HireProjection;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.HireProjection;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class HireProjectionsControllerTest
    {
        private HireProjectionsController controller;
        private Mock<ILog<HireProjectionsController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IHireProjectionService> mockService;

        public HireProjectionsControllerTest()
        {
            mockLog = new Mock<ILog<HireProjectionsController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IHireProjectionService>();
            controller = new HireProjectionsController(mockService.Object, mockLog.Object, mockMapper.Object);
        }


        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllHireProjections()
        {
            var expectedValue = new List<ReadedHireProjectionViewModel>();
            expectedValue.Add(new ReadedHireProjectionViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedHireProjectionContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedHireProjectionViewModel>>(It.IsAny<IEnumerable<ReadedHireProjectionContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedHireProjectionViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedHireProjectionViewModel>>(It.IsAny<IEnumerable<ReadedHireProjectionContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_HireProjectionById_WhenDataIsValid()
        {
            var hireProjectionId = 0;
            var expectedValue = new ReadedHireProjectionViewModel
            {
                Id = 0,
                Month = 1,
                Year = 2020,
                Value = 500
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedHireProjectionContract());
            mockMapper.Setup(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>())).Returns(expectedValue);

            var result = controller.Get(hireProjectionId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedHireProjectionViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_HireProjectionById_WhenDataIsInvalid()
        {
            var hireProjectionId = 0;
            var expectedValue = hireProjectionId;

            var result = controller.Get(hireProjectionId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedHireProjectionViewModel>(It.IsAny<ReadedHireProjectionContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_HireProjection()
        {
            var hireProjectionVM = new CreateHireProjectionViewModel
            {
                Month = 1,
                Year = 2020,
                Value = 500
            };

            var expectedValue = new CreatedHireProjectionViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateHireProjectionContract>(It.IsAny<CreateHireProjectionViewModel>())).Returns(new CreateHireProjectionContract());
            mockMapper.Setup(_ => _.Map<CreatedHireProjectionViewModel>(It.IsAny<CreatedHireProjectionContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateHireProjectionContract>())).Returns(new CreatedHireProjectionContract());

            var result = controller.Post(hireProjectionVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedHireProjectionViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateHireProjectionContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedHireProjectionViewModel>(It.IsAny<CreatedHireProjectionContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_HireProjection()
        {
            var hireProjectionId = 0;
            var hireProjectionToUpdate = new UpdateHireProjectionViewModel();
            var expectedValue = new { id = hireProjectionId};

            mockMapper.Setup(_ => _.Map<UpdateHireProjectionContract>(It.IsAny<UpdateHireProjectionViewModel>())).Returns(new UpdateHireProjectionContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateHireProjectionContract>()));

            var result = controller.Put(hireProjectionId, hireProjectionToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateHireProjectionContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateHireProjectionContract>(It.IsAny<UpdateHireProjectionViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_HireProjection()
        {
            var hireProjectionId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(hireProjectionId);

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