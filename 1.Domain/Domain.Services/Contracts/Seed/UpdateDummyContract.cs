﻿using System;

namespace Domain.Services.Contracts.Seed
{
    public class UpdateDummyContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TestValue { get; set; }
    }
}
