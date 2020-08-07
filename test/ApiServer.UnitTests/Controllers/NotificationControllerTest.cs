// <copyright file="NotificationControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using ApiServer.Controllers;
    using Core;
    using Domain.Model;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class NotificationControllerTest
    {
        private readonly NotificationController controller;
        private readonly Mock<INotificationService> mockService;
        private readonly Mock<ILog<NotificationController>> mockLogger;

        public NotificationControllerTest()
        {
            this.mockService = new Mock<INotificationService>();
            this.mockLogger = new Mock<ILog<NotificationController>>();
            this.controller = new NotificationController(this.mockService.Object, this.mockLogger.Object);
        }

        [Fact(DisplayName = "Verify that method 'GetNotification' returns AcceptedResult")]
        public void Should_GetNotification()
        {
            var expectedValue = new List<Notification>();
            expectedValue.Add(new Notification
            {
                Id = 0,
                Text = "testNotification",
                ApplicationUserId = 0,
                ApplicationUser = null,
                IsRead = false,
                ReferredBy = string.Empty,
            });

            this.mockService.Setup(_ => _.GetNotification()).Returns(expectedValue);

            var result = this.controller.GetNotification();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<Notification>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.GetNotification(), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'ReadNotification' returns AcceptedResult")]
        public void Should_ReadNotification()
        {
            var userId = 0;

            this.mockService.Setup(_ => _.ReadNotification(It.IsAny<int>()));

            var result = this.controller.ReadNotification(userId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.ReadNotification(It.IsAny<int>()), Times.Once);
        }
    }
}
