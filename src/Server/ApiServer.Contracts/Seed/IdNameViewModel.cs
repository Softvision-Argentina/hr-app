// <copyright file="IdNameViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Seeds
{
    using System;

    public class IdNameViewModel
    {
        /// <summary>
        /// Task Type Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Task Type Name.
        /// </summary>
        public string Name { get; set; }
    }
}
