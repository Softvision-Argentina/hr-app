using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Persistance.Testing;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.FunctionalTests.Core
{
    public partial class BaseFunctionalTestFixture : IFixtureHelperAsync
    {
        public virtual async Task SeedAsync<T>(List<T> list) where T : class
        {
            list.ForEach(async (entity) => { await Context.Set<T>().AddAsync(entity);});
            await Context.SaveChangesAsync();
        }
        public virtual async Task SeedAsync<T>(T model) where T : class
        {
            await Context.Set<T>().AddAsync(model);
            await Context.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync<T>() where T : class
        {
            var list = Context.Set<T>().ToList();
            list.ForEach((entity) => { Context.Remove(entity); });
            await Context.SaveChangesAsync();
        }
        public virtual async Task<T> GetAsync<T>(int id) where T : Entity<int>
        {
            var model = await Context.Set<T>().AsNoTracking().FirstAsync(_ => _.Id == id);
            return model;
        }
        public virtual async Task<int> GetCountAsync<T>() where T : class
        {
            var list = await Context.Set<T>().AsNoTracking().ToListAsync();
            return list.Count;
        }
        public virtual Task<object> GetEagerAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
