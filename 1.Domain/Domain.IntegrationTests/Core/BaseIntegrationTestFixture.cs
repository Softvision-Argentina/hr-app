using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiServer;
using Core;
using Core.Persistance;
using Core.Persistance.Testing;
using Domain.Services.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using Persistance.EF.Extensions;

namespace Domain.Services.Impl.IntegrationTests.Core
{
    public class BaseIntegrationTestFixture : WebAppFactory, IFixtureHelper
    {
        public DataBaseContext Context { get; set; }
        public BaseIntegrationTestFixture() : base(EnvironmentType.Integration)
        {
            Context = (DataBaseContext) Server.Host.Services.GetService(typeof(DataBaseContext));
            Context.Database.EnsureCreated();
            Context.DeleteAllEntities();
            Context.ResetAllIdentitiesId();
        }

        public void Seed<T>(List<T> entities) where T : class
        {
            entities.ForEach((entity) => { Context.Set<T>().Add(entity); });
            Context.SaveChanges();
            Context.DetachAllEntities();
        }
        public void Seed<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
            Context.Entry(entity).State = EntityState.Detached;
        }
        public void Delete<T>() where T : class
        {
            var list = Context.Set<T>().ToList();
            list.ForEach((entity) => { Context.Remove(entity); });
            Context.SaveChanges();
        }
        public T Get<T>(int id) where T : Entity<int>
        {
            return Context.Set<T>().AsNoTracking().First(_ => _.Id == id);
        }
        public int GetCount<T>() where T : class
        {
            return Context.Set<T>().AsNoTracking().ToList().Count;
        }
        public virtual void DeleteEager()
        {
            throw new NotImplementedException();
        }
        public virtual T GetEager<T>(int id) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
