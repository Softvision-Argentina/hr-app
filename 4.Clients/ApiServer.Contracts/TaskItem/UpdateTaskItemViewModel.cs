using ApiServer.Contracts.Task;

namespace ApiServer.Contracts.TaskItem
{
    public class UpdateTaskItemViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public int TaskId { get; set; }
        public UpdateTaskViewModel Task { get; set; }
    }
}
