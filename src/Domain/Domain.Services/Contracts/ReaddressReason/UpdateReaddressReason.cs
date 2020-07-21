using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.ReaddressReason
{
    public class UpdateReaddressReason
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
    }
}
