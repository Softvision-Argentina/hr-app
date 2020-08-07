// <copyright file="UpdateReaddressReason.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.ReaddressReason
{
    public class UpdateReaddressReason
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }
    }
}
