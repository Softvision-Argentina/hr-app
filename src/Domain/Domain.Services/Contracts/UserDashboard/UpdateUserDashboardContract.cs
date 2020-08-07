// <copyright file="UpdateUserDashboardContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.UserDashboard
{
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Contracts.User;

    public class UpdateUserDashboardContract
    {
        public int UserId { get; set; }

        public ReadedUserContract User { get; set; }

        public int DashboardId { get; set; }

        public ReadedDashboardContract Dashboard { get; set; }
    }
}
