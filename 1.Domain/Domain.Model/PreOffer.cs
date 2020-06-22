using Core;
using Domain.Model.Enum;
using System;

namespace Domain.Model
{
    public class PreOffer: Entity<int>
    {                
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
