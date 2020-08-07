// <copyright file="CreatePostulantContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Postulant
{
    using System;

    public class CreatePostulantContract
    {
        public string Name { get; set; }

        public string EmailAdress { get; set; }

        public string LinkedInProfile { get; set; }

        public string Cv { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
