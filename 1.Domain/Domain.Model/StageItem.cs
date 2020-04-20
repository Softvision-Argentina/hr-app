using Core;

namespace Domain.Model
{
    public class StageItem : Entity<int>
    {
        public string Description { get; set; }
        public string AssociatedContent { get; set; }
        public int StageId { get; set; }
        public Stage Stage { get; set; }
    }
}
