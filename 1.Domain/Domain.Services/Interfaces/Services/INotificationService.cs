using Domain.Model;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface INotificationService
    {
       List<Notification> GetNotification();

       void ReadNotification(int id);
    }
}
