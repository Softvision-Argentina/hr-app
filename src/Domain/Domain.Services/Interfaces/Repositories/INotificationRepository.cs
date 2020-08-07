// <copyright file="INotificationRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface INotificationRepository
    {
        List<Notification> GetUserNotifications(string userNotified);

        void Create(Notification notification, int user);

        void ReadNotification(int notificationId, string userNotified);
    }
}
