// <copyright file="CreateUserDashboardContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.UserDashboard
{
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Contracts.User;

    public class CreateUserDashboardContract
    {
        public int UserId { get; set; }

        public CreateUserContract User { get; set; }

        public int DashboardId { get; set; }

        public CreateDashboardContract Dashboard { get; set; }
    }
}
