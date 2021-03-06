﻿// <copyright file="CreateHireProjectionContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.HireProjection
{
    public class CreateHireProjectionContract
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
