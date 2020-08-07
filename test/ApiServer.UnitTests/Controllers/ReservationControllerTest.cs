// <copyright file="ReservationControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Reservation;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Reservation;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ReservationControllerTest
    {
        private readonly ReservationController controller;
        private readonly Mock<ILog<ReservationController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IReservationService> mockService;

        public ReservationControllerTest()
        {
            this.mockLog = new Mock<ILog<ReservationController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IReservationService>();
            this.controller = new ReservationController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllReservations()
        {
            var expectedValue = new List<ReadedReservationViewModel>();
            expectedValue.Add(new ReadedReservationViewModel
            {
                Id = 0,
                Description = "test",
                SinceReservation = DateTime.Now,
                UntilReservation = DateTime.Now.AddDays(1),
                User = 0,
                RoomId = 0,
                Room = null,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedReservationContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedReservationViewModel>>(It.IsAny<IEnumerable<ReadedReservationContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedReservationViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedReservationViewModel>>(It.IsAny<IEnumerable<ReadedReservationContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult")]
        public void Should_Get_ReservationById()
        {
            var reservationId = 0;
            var expectedValue = new ReadedReservationViewModel
            {
                Id = 0,
                Description = "test",
                SinceReservation = DateTime.Now,
                UntilReservation = DateTime.Now.AddDays(1),
                User = 0,
                RoomId = 0,
                Room = null,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedReservationContract());
            this.mockMapper.Setup(_ => _.Map<ReadedReservationViewModel>(It.IsAny<ReadedReservationContract>())).Returns(expectedValue);

            var result = this.controller.Get(reservationId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedReservationViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedReservationViewModel>(It.IsAny<ReadedReservationContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Reservation()
        {
            var reservationVM = new CreateReservationViewModel
            {
                Description = "test",
                SinceReservation = DateTime.Now,
                UntilReservation = DateTime.Now.AddDays(1),
                User = 0,
                RoomId = 0,
                Room = null,
            };

            var expectedValue = new CreatedReservationViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateReservationContract>(It.IsAny<CreateReservationViewModel>())).Returns(new CreateReservationContract());
            this.mockMapper.Setup(_ => _.Map<CreatedReservationViewModel>(It.IsAny<CreatedReservationContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateReservationContract>())).Returns(new CreatedReservationContract());

            var result = this.controller.Post(reservationVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedReservationViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateReservationContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedReservationViewModel>(It.IsAny<CreatedReservationContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Reservation()
        {
            var reservationId = 0;
            var reservationToUpdate = new UpdateReservationViewModel();

            this.mockMapper.Setup(_ => _.Map<UpdateReservationContract>(It.IsAny<UpdateReservationViewModel>())).Returns(new UpdateReservationContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateReservationContract>()));

            var result = this.controller.Put(reservationId, reservationToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateReservationContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateReservationContract>(It.IsAny<UpdateReservationViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Reservation()
        {
            var reservationId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(reservationId);

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