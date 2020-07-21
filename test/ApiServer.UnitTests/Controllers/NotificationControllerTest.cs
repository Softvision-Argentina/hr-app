using ApiServer.Controllers;
using Domain.Model;
using Core;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class NotificationControllerTest
    {
        private NotificationController controller;
        private Mock<INotificationService> mockService;
        private Mock<ILog<NotificationController>> mockLogger;

        public NotificationControllerTest()
        {
            mockService = new Mock<INotificationService>();
            mockLogger = new Mock<ILog<NotificationController>>();
            controller = new NotificationController(mockService.Object, mockLogger.Object);
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
                ReferredBy = ""
            });

            mockService.Setup(_ => _.GetNotification()).Returns(expectedValue);

            var result = controller.GetNotification();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<Notification>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.GetNotification(), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'ReadNotification' returns AcceptedResult")]
        public void Should_ReadNotification()
        {
            var userId = 0;

            mockService.Setup(_ => _.ReadNotification(It.IsAny<int>()));

            var result = controller.ReadNotification(userId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.ReadNotification(It.IsAny<int>()), Times.Once);
        }
    }
}
