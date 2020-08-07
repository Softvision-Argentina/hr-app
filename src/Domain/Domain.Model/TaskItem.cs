// <copyright file="TaskItem.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class TaskItem : Entity<int>
    {
        public string Text { get; set; }

        public bool Checked { get; set; }

        public int TaskId { get; set; }

        public Task Task { get; set; }
    }
}
