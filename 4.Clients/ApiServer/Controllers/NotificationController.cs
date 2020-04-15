using Core;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationController : BaseController<NotificationController>
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, ILog<NotificationController> logger)
            : base(logger)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            return ApiAction(() =>
            {
                _notificationService.GetNotification();

                return Accepted();
            });
        }

        [HttpPut("{id}")]
        public IActionResult ReadNotification(int id)
        {
            return ApiAction(() =>
            {
                _notificationService.ReadNotification(id);

                return Accepted();
            });
        }
    }
}
