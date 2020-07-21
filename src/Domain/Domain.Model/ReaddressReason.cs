using Core;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
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
