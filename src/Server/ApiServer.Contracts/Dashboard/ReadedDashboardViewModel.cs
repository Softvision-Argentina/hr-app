// <copyright file="ReadedDashboardViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Dashboard
{
    using System.Collections.Generic;
    using ApiServer.Contracts.UserDashboard;

    public class ReadedDashboardViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ReadedUserDashboardViewModel> UserDashboards { get; set; }
    }
}
