using Domain.Model.Enum;
using System;

namespace ApiServer.Contracts.PreOffer
{
    public class UpdatePreOfferViewModel
    {
        public int Id { get; set; }
        public DateTime? PreOfferDate { get; set; }
        public float Salary { get; set; }
        public PreOfferStatus Status { get; set; }
        public int VacationDays { get; set; }
        public HealthInsuranceEnum HealthInsurance { get; set; }
        public int ProcessId { get; set; }
        public int Bonus { get; set; }
        public string Notes { get; set; }
        public DateTime TentativeStartDate { get; set; }
    }
}
