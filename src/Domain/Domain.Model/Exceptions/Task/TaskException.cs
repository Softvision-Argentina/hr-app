// <copyright file="TaskException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Task
{
    using System;
    using Core;

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
            this.TaskId = taskId;
        }

        public int TaskId { get; set; }
    }

    public class TaskDeletedException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.TaskDeleted;

        public TaskDeletedException(int id, string name)
            : base($"The task {name} was deleted")
        {
            this.TaskId = id;
            this.Name = name;
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
            this.TaskId = taskId;
            this.ClientSystemId = clientSystemId;
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
            this.TaskId = taskId;
            this.ClientSystemId = clientSystemId;
        }

        public int TaskId { get; }

        public Guid ClientSystemId { get; }
    }

    public class TaskNotFoundException : InvalidTaskException
    {
        protected override int SubErrorCode => (int)TaskErrorSubCodes.TaskNotFound;

        public TaskNotFoundException(int taskId) : base($"The Task {taskId} was not found.")
        {
            this.TaskId = taskId;
        }

        public int TaskId { get; }
    }
}
