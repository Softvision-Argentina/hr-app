using System;
using Domain.Services.Impl.IntegrationTests.Core;
using Persistance.EF.Extensions;

namespace Domain.Services.Impl.IntegrationTests.Fixtures
{
    public class CandidateProfileFixture : BaseIntegrationTestFixture, IDisposable
    {
        public CandidateProfileFixture()
        {
            
        }

        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}
