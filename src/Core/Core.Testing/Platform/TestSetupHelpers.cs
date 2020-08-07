// <copyright file="TestSetupHelpers.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Platform
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Core.Testing.Models;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public static class TestHelpers
    {
        public static IConfigurationBuilder GetConfigurationBuilder(IReadOnlyCollection<JsonFileProperties> jsonFiles = null)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables("ASPNETCORE");

            if (jsonFiles == null || jsonFiles.Count <= 0)
            {
                return builder;
            }

            foreach (var jsonFile in jsonFiles)
            {
                builder.AddJsonFile(jsonFile.JsonFilePath, jsonFile.Optional, jsonFile.ReloadOnChange);
            }

            return builder;
        }

        public static TestServer GetTestServer<T>(IConfigurationBuilder builder) where T : class
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(builder.Build())
                .UseStartup<T>());

            return server;
        }

        public static string JsonizeModel<T>(T model) where T : class
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
