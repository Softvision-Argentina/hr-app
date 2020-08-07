// <copyright file="ReaddressReasonType.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core;

    public class ReaddressReasonType : Entity<int>
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<ReaddressReason> Reasons { get; set; }
    }
}
