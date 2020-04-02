using ApiServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace Domain.Services.Impl.IntegrationTests.Core
{
    public class ServiceFixture : IDisposable
    {
        private static readonly object Sync = new object();
        private static bool _configured;
        private static string _env = "IntegrationTest";
        public TestServer Server { get; internal set; }
        public HttpClient Client { get; internal set; }
        public IServiceProvider Services { get; internal set; }

        public ServiceFixture()
        {
            lock (Sync)
            {
                if (!_configured)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.{_env}.json", optional: false, reloadOnChange: true);

                    Server = new TestServer(WebHost.CreateDefaultBuilder()
                        .UseEnvironment(_env)
                        .UseConfiguration(builder.Build())
                        .UseStartup<Startup>());

                    Services = Server.Host.Services;
                    Client = Server.CreateClient();
                    _configured = true;
                }
            }
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }

    [CollectionDefinition("Service collection")]
    public class ServiceCollection : ICollectionFixture<ServiceFixture>
    {

    }
}
