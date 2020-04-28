using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistance.Testing
{
    public interface IFixtureHelperAsync
    {
        Task SeedAsync<T>(List<T> model) where T : class;
        Task SeedAsync<T>(T model) where T : class;
        Task DeleteAsync<T>() where T : class;
        Task<T> GetAsync<T>(int id) where T : Entity<int>;
        Task<int> GetCountAsync<T>() where T : class;
        Task DeleteEagerAsync();
        Task<object> GetEagerAsync(int id);
    }
}
