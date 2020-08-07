// <copyright file="PreOfferControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.PreOffer;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.PreOffer;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class PreOfferControllerTest
    {
        private readonly PreOfferController controller;
        private readonly Mock<ILog<PreOfferController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IPreOfferService> mockService;

        public PreOfferControllerTest()
        {
            this.mockLog = new Mock<ILog<PreOfferController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IPreOfferService>();
            this.controller = new PreOfferController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllPreOffers()
        {
            var expectedValue = new List<ReadedPreOfferViewModel>();
            expectedValue.Add(new ReadedPreOfferViewModel
            {
                Id = 0,
                PreOfferDate = DateTime.Now,
                Salary = 500,
                Status = PreOfferStatus.Accepted,
                VacationDays = 10,
                HealthInsurance = HealthInsuranceEnum.OSDE210,
                ProcessId = 0,
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedPreOfferContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedPreOfferViewModel>>(It.IsAny<IEnumerable<ReadedPreOfferContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedPreOfferViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedPreOfferViewModel>>(It.IsAny<IEnumerable<ReadedPreOfferContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_OfferById_WhenDataIsValid()
        {
            var preOfferId = 0;
            var expectedValue = new ReadedPreOfferViewModel
            {
                Id = 0,
                PreOfferDate = DateTime.Now,
                Salary = 500,
                Status = PreOfferStatus.Accepted,
                VacationDays = 10,
                HealthInsurance = HealthInsuranceEnum.OSDE210,
                ProcessId = 0,
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedPreOfferContract());
            this.mockMapper.Setup(_ => _.Map<ReadedPreOfferViewModel>(It.IsAny<ReadedPreOfferContract>())).Returns(expectedValue);

            var result = this.controller.Get(preOfferId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedPreOfferViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedPreOfferViewModel>(It.IsAny<ReadedPreOfferContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_PreOfferById_WhenDataIsInvalid()
        {
            var preOfferId = 0;
            var expectedValue = preOfferId;

            var result = this.controller.Get(preOfferId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedPreOfferViewModel>(It.IsAny<ReadedPreOfferContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_PreOffer()
        {
            var preOfferVM = new CreatePreOfferViewModel
            {
                PreOfferDate = DateTime.Now,
                Salary = 500,
                Status = PreOfferStatus.Accepted,
                VacationDays = 10,
                HealthInsurance = HealthInsuranceEnum.OSDE210,
                ProcessId = 0,
            };

            var expectedValue = new CreatedPreOfferViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreatePreOfferContract>(It.IsAny<CreatePreOfferViewModel>())).Returns(new CreatePreOfferContract());
            this.mockMapper.Setup(_ => _.Map<CreatedPreOfferViewModel>(It.IsAny<CreatedPreOfferContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreatePreOfferContract>())).Returns(new CreatedPreOfferContract());

            var result = this.controller.Post(preOfferVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedPreOfferViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreatePreOfferContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedPreOfferViewModel>(It.IsAny<CreatedPreOfferContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_PreOffer()
        {
            var preOfferId = 0;
            var preOfferToUpdate = new UpdatePreOfferViewModel();
            var expectedValue = new { id = preOfferId };

            this.mockMapper.Setup(_ => _.Map<UpdatePreOfferContract>(It.IsAny<UpdatePreOfferViewModel>())).Returns(new UpdatePreOfferContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdatePreOfferContract>()));

            var result = this.controller.Put(preOfferId, preOfferToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdatePreOfferContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdatePreOfferContract>(It.IsAny<UpdatePreOfferViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_PreOffer()
        {
            var preOfferId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(preOfferId);

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