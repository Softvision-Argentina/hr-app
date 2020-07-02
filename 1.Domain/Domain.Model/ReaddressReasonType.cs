using Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
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
