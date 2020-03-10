using Core;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

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
