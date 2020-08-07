// <copyright file="UpdateUserDashboardViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.UserDashboard
{
    using ApiServer.Contracts.Dashboard;
    using ApiServer.Contracts.User;

    public class UpdateUserDashboardViewModel
    {
        public int UserId { get; set; }

        public UpdateUserViewModel User { get; set; }

        public int DashboardId { get; set; }

        public UpdateDashboardViewModel Dashboard { get; set; }
    }
}
