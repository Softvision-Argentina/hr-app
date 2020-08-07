// <copyright file="UpdateTaskItemContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.TaskItem
{
    using Domain.Services.Contracts.Task;

    public class UpdateTaskItemContract
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        public int TaskId { get; set; }

        public UpdateTaskContract Task { get; set; }
    }
}
