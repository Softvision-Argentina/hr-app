using Core;
using Domain.Model.Enum;
using System;

namespace Domain.Model
{
    public class Offer: Entity<int>
    {                
        public DateTime? OfferDate { get; set; }
        public float Salary { get; set; }
        public string RejectionReason { get; set; }          
        public OfferStatus Status { get; set; }
        public int ProcessId { get; set; }
    }
}
