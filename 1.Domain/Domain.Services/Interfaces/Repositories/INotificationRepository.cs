using Domain.Model;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        List<Notification> GetUserNotifications(string userNotified);
        void Create(Notification notification, int user);
        void ReadNotification(int notificationId, string userNotified);
    }
}
