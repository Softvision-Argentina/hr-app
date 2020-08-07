// <copyright file="NotificationRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;

    public class NotificationRepository : INotificationRepository
    {
        private readonly DataBaseContext context;
        private readonly IRepository<Candidate> cand;

        public NotificationRepository(DataBaseContext context, IRepository<Candidate> cand)
        {
            this.context = context;
            this.cand = cand;
        }

        public void Create(Notification notification, int candidateId)
        {
            var candList = this.GetReferralsList(candidateId);
            foreach (var cand in candList)
            {
                notification.ApplicationUserId = cand.Id;
                notification.Id = notification.Id;
                notification.ReferredBy = cand.ReferredBy;

                this.context.Notifications.Add(notification);
                this.context.SaveChanges();
            }
        }

        private List<Candidate> GetReferralsList(int candidateId)
        {
            return this.context.Candidates.Where(w => w.Id == candidateId).ToList();
        }

        public List<Notification> GetUserNotifications(string notifiedUser)
        {
            return this.context.Notifications.Where(u => u.ReferredBy.Equals(notifiedUser) && !u.IsRead).ToList();
        }

        public void ReadNotification(int notificationId, string userId)
        {
            var notification = this.context.Notifications.Find(notificationId);

            notification.IsRead = true;
            this.context.Notifications.Update(notification);
            this.context.SaveChanges();
        }
    }
}
