// <copyright file="CreateDashboardViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Dashboard
{
    using System.Collections.Generic;
    using ApiServer.Contracts.UserDashboard;

    public class CreateDashboardViewModel
    {
        public string Name { get; set; }

        public ICollection<CreateUserDashboardViewModel> UserDashboards { get; set; }
    }
}
