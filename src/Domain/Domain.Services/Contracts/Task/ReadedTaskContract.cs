// <copyright file="ReadedTaskContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Task
{
    using System;
    using System.Collections.Generic;
    using Domain.Services.Contracts.TaskItem;
    using Domain.Services.Contracts.User;

    public class ReadedTaskContract
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsApprove { get; set; }

        public bool IsNew { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime EndDate { get; set; }

        public int UserId { get; set; }

        public ReadedUserContract User { get; set; }

        public ICollection<ReadedTaskItemContract> TaskItems { get; set; }
    }
}
