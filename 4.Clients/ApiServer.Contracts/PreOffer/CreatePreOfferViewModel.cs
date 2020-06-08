using Domain.Model.Enum;
using System;

namespace ApiServer.Contracts.PreOffer
{
    public class CreatePreOfferViewModel
    {
        public DateTime? PreOfferDate { get; set; }
        public float Salary { get; set; }
        public PreOfferStatus Status { get; set; }
        public int VacationDays { get; set; }
        public HealthInsuranceEnum HealthInsurance { get; set; }
        public int ProcessId { get; set; }
    }
}
