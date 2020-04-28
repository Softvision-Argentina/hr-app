using Domain.Services.Contracts.Task;

namespace Domain.Services.Contracts.TaskItem
{
    public class CreateTaskItemContract
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
        public int TaskId { get; set; }
        public CreateTaskContract Task { get; set; }
    }
}
