// <copyright file="Ordering.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Persistance
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class Ordering<T> where T : IEntity
    {
        private readonly Func<IQueryable<T>, IOrderedQueryable<T>> transform;

        private Ordering(Func<IQueryable<T>, IOrderedQueryable<T>> transform)
        {
            this.transform = transform;
        }

        public static Ordering<T> OrderBy<TKey>(
            Expression<Func<T, TKey>> primary)
        {
            return new Ordering<T>(query => query.OrderBy(primary));
        }

        public static Ordering<T> OrderByDescending<TKey>(
            Expression<Func<T, TKey>> primary)
        {
            return new Ordering<T>(query => query.OrderByDescending(primary));
        }

        public Ordering<T> ThenBy<TKey>(Expression<Func<T, TKey>> secondary)
        {
            return new Ordering<T>(query => this.transform(query).ThenBy(secondary));
        }

        public Ordering<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> secondary)
        {
            return new Ordering<T>(query => this.transform(query).ThenByDescending(secondary));
        }

        public IOrderedQueryable<T> Apply(IQueryable<T> query)
        {
            return this.transform(query);
        }
    }
}
