using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore;
using Xunit;
using Domain.Services.Repositories.EF;
using Persistance.EF.Extensions;

namespace ApiServer.FunctionalTests.Core
{
    public class ApiFixture : IDisposable
    {
        private static readonly object Sync = new object();
        private static bool _configured;
        private static string _env = "IntegrationTest";
        public TestServer Server { get; internal set; }
        public HttpClient Client { get; internal set; }
        public DataBaseContext Context { get; internal set; }
        public IServiceProvider Services { get; internal set; }

        public ApiFixture()
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
                    Context = Server.Host.Services.GetService(typeof(DataBaseContext)) as DataBaseContext;
                    Client = Server.CreateClient();

                    _configured = true;
                }
            }
        }

        public void Dispose()
        {
            Context.ResetAllIdentitiesId();
            Client.Dispose();
            Server.Dispose();
            Context.Dispose();
        }
    }

    [CollectionDefinition("Api collection")]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {

    }
}
