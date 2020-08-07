// <copyright file="ReadedPostulantContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Postulant
{
    using System;

    public class ReadedPostulantContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string LinkedInProfile { get; set; }

        public string Cv { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
