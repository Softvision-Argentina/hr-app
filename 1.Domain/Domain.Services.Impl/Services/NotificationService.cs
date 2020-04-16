using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Domain.Services.Interfaces.Services;
using System.Collections.Generic;
using Core.Persistance;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRepository<User> _usersRepo;

        public NotificationService(
            INotificationRepository notificationRepository,
            IHttpContextAccessor httpContext,
            IRepository<User> userRepo)
        {
            _notificationRepository = notificationRepository;
            _httpContext = httpContext;
            _usersRepo = userRepo;
        }

        public List<Notification> GetNotification()
        {
            var userName = GetUserName();

            var notification = _notificationRepository.GetUserNotifications(userName);

            return notification;
        }

        public void ReadNotification(int id)
        {
            var userName = GetUserName();

            _notificationRepository.ReadNotification(id, userName);

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
