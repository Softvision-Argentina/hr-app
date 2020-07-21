using Domain.Services.Contracts.TaskItem;
using Domain.Services.Contracts.User;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Task
{
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
