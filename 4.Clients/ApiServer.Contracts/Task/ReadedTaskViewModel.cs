using ApiServer.Contracts.TaskItem;
using ApiServer.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.Task
{
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
