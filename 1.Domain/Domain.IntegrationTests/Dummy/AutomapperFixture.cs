using AutoMapper;
using System;
using Xunit;

namespace Domain.Services.Impl.IntegrationTests.Dummy
{
    public class AutomapperFixture : IDisposable
    {
        private static readonly object Sync = new object();
        private static bool _configured;

        public AutomapperFixture()
        {
            lock (Sync)
            {
                if (!_configured)
                {
                    var config = new MapperConfiguration(cfg => cfg.AddProfiles(new[] {
                            "Domain.Services",
                            "Domain.Services.Impl"
                        }));

                    Mapper = config.CreateMapper();

                    _configured = true;
                }
            }
        }

        public IMapper Mapper { get; internal set; }

        public void Dispose()
        {

        }
    }

    [CollectionDefinition("Service Test Collection")]
    public class TestCollection : ICollectionFixture<AutomapperFixture>
    {

    }
}
