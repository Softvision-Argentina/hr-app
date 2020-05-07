using System.Collections.Generic;

namespace Core.Persistance.Testing
{
    public interface IFixtureHelper
    {
        void Seed<T>(List<T> entities) where T : class;
        void Seed<T>(T entity) where T : class;
        void Delete<T>() where T : class;
        T Get<T>(int id) where T : Entity<int>;
        int GetCount<T>() where T : class;
        T GetEager<T>(int id) where T : class;
    }
}
