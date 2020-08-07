// <copyright file="ReaddressReason.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.ComponentModel.DataAnnotations;
    using Core;

    public class ReaddressReason : Entity<int>
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required]
        public string Description { get; set; }

        [Required]
        public ReaddressReasonType Type { get; set; }
    }
}
