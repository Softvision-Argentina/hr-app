// <copyright file="UpdateTaskItemViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.TaskItem
{
    using ApiServer.Contracts.Task;

    public class UpdateTaskItemViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        public int TaskId { get; set; }

        public UpdateTaskViewModel Task { get; set; }
    }
}
