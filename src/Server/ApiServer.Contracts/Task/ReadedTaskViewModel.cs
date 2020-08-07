// <copyright file="ReadedTaskViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Task
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.TaskItem;
    using ApiServer.Contracts.User;

    public class ReadedTaskViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsApprove { get; set; }

        public bool IsNew { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime EndDate { get; set; }

        public int UserId { get; set; }

        public ReadedUserViewModel User { get; set; }

        public ICollection<ReadedTaskItemViewModel> TaskItems { get; set; }
    }
}
