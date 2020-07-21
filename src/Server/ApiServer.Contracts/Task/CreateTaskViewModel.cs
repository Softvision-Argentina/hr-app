using ApiServer.Contracts.TaskItem;
using ApiServer.Contracts.User;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Task
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public bool IsApprove { get; set; }
        public bool IsNew { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public CreateUserViewModel User { get; set; }
        public ICollection<CreateTaskItemViewModel> TaskItems { get; set; }
    }
}
