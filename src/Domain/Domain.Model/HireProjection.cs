// <copyright file="HireProjection.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class HireProjection : Entity<int>
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
