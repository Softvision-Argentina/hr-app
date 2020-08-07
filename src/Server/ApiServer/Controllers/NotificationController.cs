// <copyright file="NotificationController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using Core;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/Notifications")]
    [ApiController]
    public class NotificationController : BaseController<NotificationController>
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService, ILog<NotificationController> logger)
            : base(logger)
        {
            this.notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            return this.ApiAction(() =>
           {
               var notifications = this.notificationService.GetNotification();

               return this.Accepted(notifications);
           });
        }

        [HttpPut("{id}")]
        public IActionResult ReadNotification(int id)
        {
            return this.ApiAction(() =>
            {
                this.notificationService.ReadNotification(id);

                return this.Accepted();
            });
        }
    }
}
