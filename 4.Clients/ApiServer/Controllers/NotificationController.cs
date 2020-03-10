using Domain.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut("{id}")]
        public IActionResult ReadNotification(int id)
        {
            var userName = GetUserName();

            _notificationRepository.ReadNotification(id, userName);
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
