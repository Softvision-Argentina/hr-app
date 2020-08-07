// <copyright file="ReadedUserDashboardViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.UserDashboard
{
    using ApiServer.Contracts.Dashboard;
    using ApiServer.Contracts.User;

    public class ReadedUserDashboardViewModel
    {
        public int UserId { get; set; }

        public ReadedUserViewModel User { get; set; }

        public int DashboardId { get; set; }

        public ReadedDashboardViewModel Dashboard { get; set; }
    }
}
