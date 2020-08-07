// <copyright file="Dummy.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Seed
{
    using System;
    using Core;

    public class Dummy : DescriptiveEntity<Guid>
    {
        public string TestValue { get; set; }
    }
}
