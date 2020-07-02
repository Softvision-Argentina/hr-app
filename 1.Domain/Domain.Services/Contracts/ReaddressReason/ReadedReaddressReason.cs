using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.ReaddressReason
{
    public class ReadReaddressReason
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

    }
}
