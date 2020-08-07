// <copyright file="ISecurityTokenProvider.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface ISecurityTokenProvider
    {
        string BuildSecurityToken(string userName, List<Claim> claims);
    }
}
