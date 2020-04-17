using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Persistance
{
    public interface IRepository
    {
    }

    /// <summary>
    /// This repository is suitable for wrapping regular ORMs, which commonly offer the IQueryable collection.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity</typeparam>
    public interface IRepository<TEntity> : IRepository where TEntity : IEntity
    {
        IQueryable<TEntity> Query();
        IQueryable<TEntity> QueryEager();
        TEntity Get<TKey>(TKey id) where TKey : IComparable, IFormattable;
        Task<TEntity> GetAsync<TKey>(TKey id) where TKey : IComparable, IFormattable;
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        int Count();
    }
}
