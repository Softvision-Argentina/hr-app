// <copyright file="ReadedUserViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.User
{
    using ApiServer.Contracts.Community;

    public class ReadedUserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public ReadedCommunityViewModel Community { get; set; }
    }
}
