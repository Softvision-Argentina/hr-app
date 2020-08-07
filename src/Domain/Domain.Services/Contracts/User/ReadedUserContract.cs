// <copyright file="ReadedUserContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.User
{
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Community;

    public class ReadedUserContract
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public Roles Role { get; set; }

        public string Password { get; set; }

        public ReadedCommunityContract Community { get; set; }
    }
}
