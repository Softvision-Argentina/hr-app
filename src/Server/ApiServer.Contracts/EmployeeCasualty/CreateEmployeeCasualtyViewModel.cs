// <copyright file="CreateEmployeeCasualtyViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.EmployeeCasualty
{
    public class CreateEmployeeCasualtyViewModel
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
