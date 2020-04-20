using Core;
using System;

namespace Domain.Model.Seed
{
    public class Dummy: DescriptiveEntity<Guid>
    {
        public string TestValue { get; set; }
    }
}
