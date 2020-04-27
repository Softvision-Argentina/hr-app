using ApiServer.Contracts.Offer;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Model.Enum;
using Domain.Services.Contracts.Offer;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class OfferControllerTest
    {
        private OfferController controller;
        private Mock<ILog<OfferController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IOfferService> mockService;

        public OfferControllerTest()
        {
            mockLog = new Mock<ILog<OfferController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IOfferService>();
            controller = new OfferController(mockService.Object, mockLog.Object, mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllOffers()
        {
            var expectedValue = new List<ReadedOfferViewModel>();
            expectedValue.Add(new ReadedOfferViewModel
            {
                Id = 0,
                OfferDate = DateTime.Now,
                Salary = 500,
                RejectionReason = "testing",
                Status = OfferStatus.Accepted,
                ProcessId = 0
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedOfferContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedOfferViewModel>>(It.IsAny<IEnumerable<ReadedOfferContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedOfferViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedOfferViewModel>>(It.IsAny<IEnumerable<ReadedOfferContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_OfferById_WhenDataIsValid()
        {
            var offerId = 0;
            var expectedValue = new ReadedOfferViewModel
            {
                Id = 0,
                OfferDate = DateTime.Now,
                Salary = 500,
                RejectionReason = "testing",
                Status = OfferStatus.Accepted,
                ProcessId = 0
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedOfferContract());
            mockMapper.Setup(_ => _.Map<ReadedOfferViewModel>(It.IsAny<ReadedOfferContract>())).Returns(expectedValue);

            var result = controller.Get(offerId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedOfferViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfferViewModel>(It.IsAny<ReadedOfferContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_OfferById_WhenDataIsInvalid()
        {
            var offerId = 0;
            var expectedValue = offerId;

            var result = controller.Get(offerId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfferViewModel>(It.IsAny<ReadedOfferContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Offer()
        {
            var offerVM = new CreateOfferViewModel
            {
                OfferDate = DateTime.Now,
                Salary = 500,
                RejectionReason = "testing",
                Status = OfferStatus.Accepted,
                ProcessId = 0
            };

            var expectedValue = new CreatedOfferViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateOfferContract>(It.IsAny<CreateOfferViewModel>())).Returns(new CreateOfferContract());
            mockMapper.Setup(_ => _.Map<CreatedOfferViewModel>(It.IsAny<CreatedOfferContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateOfferContract>())).Returns(new CreatedOfferContract());

            var result = controller.Post(offerVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedOfferViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateOfferContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedOfferViewModel>(It.IsAny<CreatedOfferContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Offer()
        {
            var offerId = 0;
            var offerToUpdate = new UpdateOfferViewModel();
            var expectedValue = new { id = offerId};

            mockMapper.Setup(_ => _.Map<UpdateOfferContract>(It.IsAny<UpdateOfferViewModel>())).Returns(new UpdateOfferContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateOfferContract>()));

            var result = controller.Put(offerId, offerToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateOfferContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateOfferContract>(It.IsAny<UpdateOfferViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Offer()
        {
            var offerId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(offerId);

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