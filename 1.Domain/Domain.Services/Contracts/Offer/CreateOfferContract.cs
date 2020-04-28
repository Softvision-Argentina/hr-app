using Domain.Model.Enum;
using System;

namespace Domain.Services.Contracts.Offer
{
    public class CreateOfferContract
    {
        public DateTime? OfferDate { get; set; }
        public float Salary { get; set; }
        public string RejectionReason { get; set; }
        public OfferStatus Status { get; set; }
        public int ProcessId { get; set; }
    }
}
