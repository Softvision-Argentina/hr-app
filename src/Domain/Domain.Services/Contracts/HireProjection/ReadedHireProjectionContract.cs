// <copyright file="ReadedHireProjectionContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.HireProjection
{
    public class ReadedHireProjectionContract
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
