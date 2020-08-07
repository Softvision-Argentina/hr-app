// <copyright file="SuccessAuthData.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.User
{
    public class SuccessAuthData
    {
        public string Token { get; set; }

        public ReadedUserViewModel User { get; set; }
    }
}
