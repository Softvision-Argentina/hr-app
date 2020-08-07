// <copyright file="CreateUserDashboardViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.UserDashboard
{
    using ApiServer.Contracts.Dashboard;
    using ApiServer.Contracts.User;

    public class CreateUserDashboardViewModel
    {
        public int UserId { get; set; }

        public CreateUserViewModel User { get; set; }

        public int DashboardId { get; set; }

        public CreateDashboardViewModel Dashboard { get; set; }
    }
}
