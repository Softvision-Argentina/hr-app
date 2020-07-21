using DependencyInjection.Config;

namespace Core.Persistance
{
    public interface IMigrator
    {
        void Migrate(DatabaseConfigurations dbConfig);
    }
}
