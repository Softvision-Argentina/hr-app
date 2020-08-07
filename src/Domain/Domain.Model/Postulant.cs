// <copyright file="Postulant.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class Postulant : Entity<int>
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string LinkedInProfile { get; set; }

        public string Cv { get; set; }
    }
}
