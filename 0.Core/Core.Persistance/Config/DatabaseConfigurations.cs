// TODO: Change namespace to match folder/project structure
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
            InMemoryMode = inMemoryMode;
            RunMigrations = runMigrations;
            RunSeed = runSeed;
            ConnectionString = connectionString;
            ConnectionStringTesting = connectionStringTesting;
        }
    }
}
