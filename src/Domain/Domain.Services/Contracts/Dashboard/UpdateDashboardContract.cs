// <copyright file="UpdateDashboardContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Dashboard
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.UserDashboard;

    public class UpdateDashboardContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CreateUserDashboardContract> UserDashboards { get; set; }
    }
}
