using System;

namespace ApiServer.Contracts.Seed
{
    public class ReadedDummyViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TestValue { get; set; }
    }
}
