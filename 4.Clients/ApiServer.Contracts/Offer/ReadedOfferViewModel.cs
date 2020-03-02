using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.Offer
{
    public class ReadedOfferViewModel
    {
        public int Id { get; set; }
        public DateTime? OfferDate { get; set; }
        public float Salary { get; set; }
        public string RejectionReason { get; set; }
        public OfferStatus Status { get; set; }
        public int ProcessId { get; set; }
    }
}
