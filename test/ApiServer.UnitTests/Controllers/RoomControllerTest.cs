// <copyright file="RoomControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Reservation;
    using ApiServer.Contracts.Room;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Room;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class RoomControllerTest
    {
        private readonly RoomController controller;
        private readonly Mock<ILog<RoomController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRoomService> mockService;

        public RoomControllerTest()
        {
            this.mockLog = new Mock<ILog<RoomController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IRoomService>();
            this.controller = new RoomController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllRooms()
        {
            var expectedValue = new List<ReadedRoomViewModel>();
            expectedValue.Add(new ReadedRoomViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                OfficeId = 0,
                Office = null,
                ReservationItems = new List<ReadedReservationViewModel>(),
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedRoomContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedRoomViewModel>>(It.IsAny<IEnumerable<ReadedRoomContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedRoomViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedRoomViewModel>>(It.IsAny<IEnumerable<ReadedRoomContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_RoomById_WhenDataIsValid()
        {
            var roomId = 0;
            var expectedValue = new ReadedRoomViewModel
            {
                Id = 0,
                Name = "test",
                Description = "test",
                OfficeId = 0,
                Office = null,
                ReservationItems = new List<ReadedReservationViewModel>(),
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedRoomContract());
            this.mockMapper.Setup(_ => _.Map<ReadedRoomViewModel>(It.IsAny<ReadedRoomContract>())).Returns(expectedValue);

            var result = this.controller.Get(roomId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedRoomViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoomViewModel>(It.IsAny<ReadedRoomContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult")]
        public void Should_Get_RoomById_WhenDataIsInvalid()
        {
            var roomId = 0;
            var expectedValue = roomId;

            var result = this.controller.Get(roomId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoomViewModel>(It.IsAny<ReadedRoomContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Add' returns CreatedResult")]
        public void Should_Post_Room()
        {
            var roomVM = new CreateRoomViewModel
            {
                Name = "test",
                Description = "test",
                OfficeId = 0,
                Office = null,
                ReservationItems = new List<CreateReservationViewModel>(),
            };

            var expectedValue = new CreatedRoomViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateRoomContract>(It.IsAny<CreateRoomViewModel>())).Returns(new CreateRoomContract());
            this.mockMapper.Setup(_ => _.Map<CreatedRoomViewModel>(It.IsAny<CreatedRoomContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateRoomContract>())).Returns(new CreatedRoomContract());

            var result = this.controller.Post(roomVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedRoomViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateRoomContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedRoomViewModel>(It.IsAny<CreatedRoomContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Room()
        {
            var roomId = 0;
            var expectedValue = new { id = roomId };
            var roomToUpdate = new UpdateRoomViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateRoomContract>(It.IsAny<UpdateRoomViewModel>())).Returns(new UpdateRoomContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateRoomContract>()));

            var result = this.controller.Put(roomId, roomToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateRoomContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateRoomContract>(It.IsAny<UpdateRoomViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Room()
        {
            var roomId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(roomId);

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
