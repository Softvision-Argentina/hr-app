// <copyright file="NotificationService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Http;

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IHttpContextAccessor httpContext;
        private readonly IRepository<User> usersRepo;

        public NotificationService(
            INotificationRepository notificationRepository,
            IHttpContextAccessor httpContext,
            IRepository<User> userRepo)
        {
            this.notificationRepository = notificationRepository;
            this.httpContext = httpContext;
            this.usersRepo = userRepo;
        }

        public List<Notification> GetNotification()
        {
            var userName = this.GetUserName();

            var notification = this.notificationRepository.GetUserNotifications(userName);

            return notification;
        }

        public void ReadNotification(int id)
        {
            var userName = this.GetUserName();

            this.notificationRepository.ReadNotification(id, userName);
        }

        private string GetUserName()
        {
            var user = this.httpContext.HttpContext.User.Identity.Name;
            var userId = int.Parse(user);
            var userName = this.usersRepo.Query().FirstOrDefault(x => x.Id == userId);
            var name = $"{userName.FirstName}" + " " + $"{userName.LastName}";

            return name;
        }
    }
}
