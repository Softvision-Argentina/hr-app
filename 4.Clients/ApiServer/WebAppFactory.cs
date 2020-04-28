using System;
using System.Net.Http;
using Core.Persistance.Testing;
using Microsoft.AspNetCore.TestHost;

namespace ApiServer
{
    public class WebAppFactory
    {
        public TestServer Server { get; internal set; }
        public HttpClient Client { get; internal set; }
        public IServiceProvider Services { get; internal set; }

        public WebAppFactory(string environment)
        {
            var builder = TestHelpers.GetConfigurationBuilder(environment);
            Server = TestHelpers.GetTestServer<Startup>(environment, builder);
            Client = Server.CreateClient();
            Services = Server.Host.Services;
        }
    }
}
