// <copyright file="CreateHireProjectionViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.HireProjection
{
    public class CreateHireProjectionViewModel
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Value { get; set; }
    }
}
