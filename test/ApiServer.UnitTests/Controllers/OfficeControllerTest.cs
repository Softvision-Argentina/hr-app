using ApiServer.Contracts.Office;
using ApiServer.Contracts.Room;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.Office;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class OfficeControllerTest
    {
        private OfficeController controller;
        private Mock<ILog<OfficeController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IOfficeService> mockService;

        public OfficeControllerTest()
        {
            mockLog = new Mock<ILog<OfficeController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IOfficeService>();
            controller = new OfficeController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllOffices()
        {
            var expectedValue = new List<ReadedOfficeViewModel>();
            expectedValue.Add(new ReadedOfficeViewModel
            {
                Id = 0,
                Name = "testOffice",
                Description = "test office",
                RoomItems = new Collection<ReadedRoomViewModel>()
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedOfficeContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedOfficeViewModel>>(It.IsAny<IEnumerable<ReadedOfficeContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedOfficeViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedOfficeViewModel>>(It.IsAny<IEnumerable<ReadedOfficeContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_OfficeById_WhenDataIsValid()
        {
            var officeId = 0;
            var expectedValue = new ReadedOfficeViewModel
            {
                Id = 0,
                Name = "testOffice",
                Description = "test office",
                RoomItems = new Collection<ReadedRoomViewModel>()
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedOfficeContract());
            mockMapper.Setup(_ => _.Map<ReadedOfficeViewModel>(It.IsAny<ReadedOfficeContract>())).Returns(expectedValue);

            var result = controller.Get(officeId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedOfficeViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfficeViewModel>(It.IsAny<ReadedOfficeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_OfficeById_WhenDataIsInvalid()
        {
            var officeId = 0;
            var expectedValue = officeId;

            var result = controller.Get(officeId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfficeViewModel>(It.IsAny<ReadedOfficeContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Office()
        {
            var officeVM = new CreateOfficeViewModel
            {
                Name = "testOffice",
                Description = "test office",
                RoomItems = new Collection<CreateRoomViewModel>()
            };

            var expectedValue = new CreatedOfficeViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateOfficeContract>(It.IsAny<CreateOfficeViewModel>())).Returns(new CreateOfficeContract());
            mockMapper.Setup(_ => _.Map<CreatedOfficeViewModel>(It.IsAny<CreatedOfficeContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateOfficeContract>())).Returns(new CreatedOfficeContract());

            var result = controller.Post(officeVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedOfficeViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateOfficeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedOfficeViewModel>(It.IsAny<CreatedOfficeContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Office()
        {
            var officeId = 0;
            var officeToUpdate = new UpdateOfficeViewModel();
            var expectedValue = new { id = officeId };

            mockMapper.Setup(_ => _.Map<UpdateOfficeContract>(It.IsAny<UpdateOfficeViewModel>())).Returns(new UpdateOfficeContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateOfficeContract>()));

            var result = controller.Put(officeId, officeToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateOfficeContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateOfficeContract>(It.IsAny<UpdateOfficeViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Office()
        {
            var officeId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(officeId);

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
