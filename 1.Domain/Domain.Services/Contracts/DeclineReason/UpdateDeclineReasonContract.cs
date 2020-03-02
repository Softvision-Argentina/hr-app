using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts
{
    public class UpdateDeclineReasonContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
