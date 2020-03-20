using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore;
using Xunit;

namespace ApiServer.Tests
{
    public class ApiFixture : IDisposable
    {
        private static readonly object Sync = new object();
        private static bool _configured;
        private static string _env = "IntegrationTest";

        public ApiFixture()
        {
            lock (Sync)
            {
                if (!_configured)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{_env}.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    Server = new TestServer(WebHost.CreateDefaultBuilder()
                        .UseEnvironment(_env)
                        .UseConfiguration(builder.Build())
                        .UseStartup<Startup>());
                    Client = Server.CreateClient();
                    _configured = true;
                }
            }
        }

        public TestServer Server { get; internal set; }
        public HttpClient Client { get; internal set; }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }

    [CollectionDefinition("Api collection")]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {

    }
}
