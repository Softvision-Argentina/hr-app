using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Task: Entity<int>
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
