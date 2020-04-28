using ApiServer.Contracts.Task;

namespace ApiServer.Contracts.TaskItem
{
    public class CreateTaskItemViewModel
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
        public int TaskId { get; set; }
        public CreateTaskViewModel Task { get; set; }
    }
}
