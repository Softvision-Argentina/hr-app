// <copyright file="Role.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class Role : Entity<int>
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
