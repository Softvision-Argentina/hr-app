// <copyright file="CreateRoleContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Role
{
    public class CreateRoleContract
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
