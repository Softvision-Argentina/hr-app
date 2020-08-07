// <copyright file="CreateEmployeeCasualtyContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.EmployeeCasualty
{
    public class CreateEmployeeCasualtyContract
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
