// <copyright file="DatabaseConfigurations.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace DependencyInjection.Config
{
    public class DatabaseConfigurations
    {
        public bool InMemoryMode { get; }

        public bool RunMigrations { get; }

        public bool RunSeed { get; }

        public string ConnectionString { get; }

        public string ConnectionStringTesting { get; }

        public DatabaseConfigurations(
            bool inMemoryMode,
            bool runMigrations,
            bool runSeed,
            string connectionString = "",
            string connectionStringTesting = "")
        {
            this.InMemoryMode = inMemoryMode;
            this.RunMigrations = runMigrations;
            this.RunSeed = runSeed;
            this.ConnectionString = connectionString;
            this.ConnectionStringTesting = connectionStringTesting;
        }
    }
}
