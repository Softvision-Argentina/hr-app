using System;
using Core.Testing.Platform;
using Domain.Services.Interfaces.Services;

namespace Domain.Services.Impl.IntegrationTests.Fixtures
{
    public class CandidateProfileFixture : BaseIntegrationTestFixture, IDisposable
    {
        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
