using Core;
using Domain.Model.Enum;

namespace Domain.Model
{
    public class ReaddressStatus : Entity<int>
    {
        public StageStatus FromStatus { get; set; }
        public StageStatus ToStatus { get; set; }
        public int? ReaddressReasonId { get; set; }
        public ReaddressReason ReaddressReason { get; set; }
        public string Feedback { get; set; }
    }
}
