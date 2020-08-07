// <copyright file="CreateTaskItemContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.TaskItem
{
    using Domain.Services.Contracts.Task;

    public class CreateTaskItemContract
    {
        public string Text { get; set; }

        public bool Checked { get; set; }

        public int TaskId { get; set; }

        public CreateTaskContract Task { get; set; }
    }
}
