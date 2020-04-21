using Core;

namespace Domain.Model
{
    public class TaskItem : Entity<int>
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
