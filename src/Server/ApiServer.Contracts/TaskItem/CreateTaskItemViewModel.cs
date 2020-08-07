// <copyright file="CreateTaskItemViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.TaskItem
{
    using ApiServer.Contracts.Task;

    public class CreateTaskItemViewModel
    {
        public string Text { get; set; }

        public bool Checked { get; set; }

        public int TaskId { get; set; }

        public CreateTaskViewModel Task { get; set; }
    }
}
