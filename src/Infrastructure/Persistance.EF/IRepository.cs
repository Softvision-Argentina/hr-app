using Core;
using Core.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Persistance.EF
{
    public interface IRepository<TEntity, TContext> : IRepository, IRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
    }
}
