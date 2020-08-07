// <copyright file="ReadedUserRoleContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.User
{
    using Domain.Model.Enum;

    public class ReadedUserRoleContract
    {
        public Roles Role { get; set; }
    }
}
