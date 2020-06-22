using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ApiServer;
using Core.Testing.Interfaces;
using Domain.Services.Repositories.EF;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.EF.Extensions;

namespace Core.Testing.Platform
{
    public class WebAppFactory : ITestingHelpers
    {
        public TestServer Server { get; set; }
        public HttpClient Client { get; set; }

        public WebAppFactory()
        {
            var builder = TestHelpers.GetConfigurationBuilder();
            Server = TestHelpers.GetTestServer<StartupTesting>(builder);
            Client = Server.CreateClient();
        }

        //ITestingHelpers
        public void ContextAction(Action<DataBaseContext> action)
        {
            using (var scope = Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = (DataBaseContext) scope.ServiceProvider.GetService(typeof(DataBaseContext));
                {
                    action(context);
                }
            }
        }
        public void UseService<T>(Action<T> action) where T: class
        {
            using (var scope = Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var service = (T) scope.ServiceProvider.GetService(typeof(T));
                {
                    action(service);
                }
            }
        }
       
        public void Seed<T>(List<T> entities) where T : class
        {
            ContextAction((context) =>
            {
                entities.ForEach((entity) => { context.Set<T>().Add(entity); });
                context.SaveChanges();
               // context.DetachAllEntities();T
            });
        }
        public void Seed<T>(T entity) where T : class
        {
            ContextAction((context) =>
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            });
        }
        public T Get<T>(int id) where T : Entity<int>
        {
            T entity = null;

            ContextAction((context) =>
            {
                entity = context.Set<T>().AsNoTracking().First(_ => _.Id == id);
            });

            return entity;
        }
        public int GetCount<T>() where T : class
        {
            int count = 0;

            ContextAction((context) =>
            {
                count = context.Set<T>().AsNoTracking().ToList().Count;
            });

            return count;
        }
        public virtual T GetEager<T>(int id) where T : class
        {
            throw new NotImplementedException();
        }
        //public void CleanTestingDatabase()
        //{
        //    ContextAction((context) =>
        //    {
        //        context.DeleteAllEntities();
        //    });
        //}
    }
}
