// <copyright file="UpdateDummyContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Seed
{
    using System;

    public class UpdateDummyContract
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TestValue { get; set; }
    }
}
