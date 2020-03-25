using System;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Domain.Services.Impl.IntegrationTests.Dummy;

namespace Domain.Services.Impl.IntegrationTests.Services
{
    [Collection("Service collection")]
    public class BaseServiceIntegrationTest
    {
        protected HttpClient Client { get; }
        protected TestServer Server { get; }
        protected IServiceProvider Services { get;  }

        public BaseServiceIntegrationTest(ServiceFixture serviceFixture)
        {
            Client = serviceFixture.Client;
            Server = serviceFixture.Server;
            Services = serviceFixture.Server.Host.Services;
        }
    }
}
