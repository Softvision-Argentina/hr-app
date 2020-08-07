// <copyright file="Dashboard.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class Dashboard : Entity<int>
    {
        public string Name { get; set; }

        public IList<UserDashboard> UserDashboards { get; set; }
    }
}
