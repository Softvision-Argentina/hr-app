// <copyright file="ReadedPostulantViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Postulant
{
    using System;

    public class ReadedPostulantViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string LinkedInProfile { get; set; }

        public string Cv { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
