// <copyright file="Task.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using System.Collections.Generic;
    using Core;

    public class Task : Entity<int>
    {
        public string Title { get; set; }

        public bool IsApprove { get; set; }

        public bool IsNew { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime EndDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public IList<TaskItem> TaskItems { get; set; }
    }
}
