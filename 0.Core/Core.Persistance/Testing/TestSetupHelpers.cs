using System.Collections.Generic;
using System.IO;
using Core.Persistance.Testing.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Persistance.Testing
{
    public static class TestHelpers
    {
        public static IConfigurationBuilder GetConfigurationBuilder(string environment, IReadOnlyCollection<JsonFileProperties> jsonFiles = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);

            if (jsonFiles == null || jsonFiles.Count <= 0) return builder;

            foreach (var jsonFile in jsonFiles)
            {
                builder.AddJsonFile(jsonFile.JsonFilePath, jsonFile.Optional, jsonFile.ReloadOnChange);
            }

            return builder;
        }
        public static TestServer GetTestServer<T>(string environment, IConfigurationBuilder builder) where T : class
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment(environment)
                .UseConfiguration(builder.Build())
                .UseStartup<T>());

            return server;
        }
        public static string JsonizeModel<T>(T model) where T: class
        {
            return JsonConvert.SerializeObject(model);   
        }
    }
}
