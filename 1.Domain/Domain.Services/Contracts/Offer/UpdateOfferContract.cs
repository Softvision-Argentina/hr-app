using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.Offer
{
    public class UpdateOfferContract
    {
        public int Id { get; set; }
        public DateTime? OfferDate { get; set; }
        public float Salary { get; set; }
        public string RejectionReason { get; set; }
        public OfferStatus Status { get; set; }
        public int ProcessId { get; set; }
    }
}
