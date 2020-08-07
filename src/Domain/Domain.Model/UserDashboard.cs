// <copyright file="UserDashboard.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    public class UserDashboard
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int DashboardId { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}
