using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Domain.Services.Repositories.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            var userName = GetUserName();

            var notification = _notificationRepository.GetUserNotifications(userName);

            return Ok(notification);
        }

        [HttpGet("{id}")]
        public IActionResult ReadNotification(int notificationId)
        {
            var userName = GetUserName();

            _notificationRepository.ReadNotification(notificationId, userName);
            return Ok();
        }

        private string GetUserName()
        {
            var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var name = userName.Split("\\");
            return name[1];
        }
    }
}
