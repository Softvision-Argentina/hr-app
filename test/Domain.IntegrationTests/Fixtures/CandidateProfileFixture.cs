// <copyright file="CandidateProfileFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.IntegrationTests.Fixtures
{
    using System;
    using Core.Testing.Platform;

    public class CandidateProfileFixture : BaseIntegrationTestFixture, IDisposable
    {
        public void Dispose()
        {
            this.Client.Dispose();
            this.Server.Dispose();
        }
    }
}
