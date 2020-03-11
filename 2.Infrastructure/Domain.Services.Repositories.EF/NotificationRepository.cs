﻿using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataBaseContext _context;
        private readonly IRepository<Candidate> _cand;

        public NotificationRepository(DataBaseContext context,IRepository<Candidate> cand)
        {
            _context = context;
            _cand = cand;
        }

        public void Create(Notification notification, int candId)
        {
            //TODO: Assign notification to users 
            var candList = GetReferralsList(candId);
            foreach (var cand in candList)
            {
                notification.ApplicationUserId = cand.Id;
                notification.Id = notification.Id;
                notification.ReferredBy = cand.ReferredBy;

                _context.Notifications.Add(notification);
                _context.SaveChanges();
            }
        }

        private List<Candidate> GetReferralsList(int candId)
        {
            return _context.Candidates.Where(w => w.Id == candId).ToList();
        }

        public List<Notification> GetUserNotifications(string userNotified)
        {
            return _context.Notifications.Where(u => u.ReferredBy.Equals(userNotified) && !u.IsRead).ToList();
        }

        public void ReadNotification(int notificationId, string userId)
        {
            var notification = _context.Notifications.Find(notificationId);

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }
    }
}