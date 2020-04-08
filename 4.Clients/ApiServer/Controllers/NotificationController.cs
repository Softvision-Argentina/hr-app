using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiServer.Controllers
{
    [Route("api/Notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRepository<User> _usersRepo;

        public NotificationController(INotificationRepository notificationRepository, IHttpContextAccessor httpContext, IRepository<User> userRepo)
        {
            _notificationRepository = notificationRepository;
            _httpContext = httpContext;
            _usersRepo = userRepo;
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
            var user = _httpContext.HttpContext.User.Identity.Name;
            var userId = int.Parse(user);
            var userName = _usersRepo.Query().FirstOrDefault(x => x.Id == userId);
            var name = $"{userName.FirstName}" + " " + $"{userName.LastName}";

            return name;
        }
    }
}
