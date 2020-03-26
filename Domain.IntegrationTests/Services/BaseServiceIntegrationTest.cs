﻿using System;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Domain.Services.Repositories.EF;

namespace Domain.IntegrationTests.Services
{
    [Collection("Service collection")]
    public class BaseServiceIntegrationTest
    {
        protected HttpClient Client { get; }
        protected TestServer Server { get; }
        protected IServiceProvider Services { get;  }

        protected DataBaseContext Context { get;  }

        public BaseServiceIntegrationTest(ServiceFixture serviceFixture)
        {
            Client = serviceFixture.Client;
            Server = serviceFixture.Server;
            Services = serviceFixture.Server.Host.Services;
            Context = serviceFixture.Server.Host.Services.GetService(typeof(DataBaseContext)) as DataBaseContext;
            Context.Database.EnsureCreated();
        }
    }
}
