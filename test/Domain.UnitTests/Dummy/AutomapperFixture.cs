// <copyright file="AutomapperFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy
{
    using System;
    using AutoMapper;
    using Xunit;

    public class AutomapperFixture : IDisposable
    {
        private static readonly object Sync = new object();
        private static bool configured;

        public AutomapperFixture()
        {
            lock (Sync)
            {
                if (!configured)
                {
                    var config = new MapperConfiguration(cfg => cfg.AddProfiles(new[]
                    {
                            "Domain.Services",
                            "Domain.Services.Impl",
                    }));

                    this.Mapper = config.CreateMapper();

                    configured = true;
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
