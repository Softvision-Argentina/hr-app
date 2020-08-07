// <copyright file="INotificationService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface INotificationService
    {
        List<Notification> GetNotification();

        void ReadNotification(int id);
    }
}
