using Core;
using System;

namespace Domain.Model.Exceptions.Task
{
    public class TaskException : BusinessException
    {
        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Task;

        public TaskException(string message)
            : base(string.IsNullOrEmpty(message) ? "There is a task related error" : message)
        {
        }
    }

    public class InvalidTaskException : TaskException
    {
        public InvalidTaskException(string message)
            : base(string.IsNullOrEmpty(message) ? "The task is not valid" : message)
        {
        }
    }

    public class DeleteTaskNotFoundException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.DeleteTaskNotFound;

        public DeleteTaskNotFoundException(int taskId)
            : base($"Task not found for the TaskId: {taskId}")
        {
            TaskId = taskId;
        }

        public int TaskId { get; set; }
    }

    public class TaskDeletedException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.TaskDeleted;

        public TaskDeletedException(int id, string name)
            : base($"The task {name} was deleted")
        {
            TaskId = id;
            Name = name;
        }

        public int TaskId { get; set; }
        public string Name { get; set; }
    }

    public class InvalidUpdateException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.InvalidUpdate;

        public InvalidUpdateException(string message)
            : base($"The update request is not valid for the task.")
        {
        }
    }

    public class UpdateTaskNotFoundException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.UpdateTaskNotFound;

        public UpdateTaskNotFoundException(int taskId, Guid clientSystemId)
            : base($"Task {taskId} and Client System Id {clientSystemId} was not found.")
        {
            TaskId = taskId;
            ClientSystemId = clientSystemId;
        }

        public int TaskId { get; }
        public Guid ClientSystemId { get; }
    }

    public class UpdateHasNotChangesException : InvalidUpdateException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.UpdateHasNotChanges;

        public UpdateHasNotChangesException(int taskId, Guid clientSystemId, string name)
            : base($"Task {name} has not changes.")
        {
            TaskId = taskId;
            ClientSystemId = clientSystemId;
        }

        public int TaskId { get; }
        public Guid ClientSystemId { get; }
    }

    public class TaskNotFoundException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.TaskNotFound;

        public TaskNotFoundException(int taskId) : base($"The Task {taskId} was not found.")
        {
            TaskId = taskId;
        }

        public int TaskId { get; }
    }
}
