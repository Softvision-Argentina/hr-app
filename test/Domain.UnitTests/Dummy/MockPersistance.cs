// <copyright file="MockPersistance.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Xunit;

    public class MockPersistance<T> where T : IEntity
    {
        private readonly List<T> createdButNotPersisted;
        private readonly List<T> updatedButNotPersisted;
        private readonly List<T> updatedAndPersisted;

        internal int PersistanceCount => this.CreatedAndPersisted.Count + this.updatedAndPersisted.Count;

        public List<T> CreatedAndPersisted { get; set; }

        internal MockPersistance()
        {
            this.CreatedAndPersisted = new List<T>();
            this.createdButNotPersisted = new List<T>();
            this.updatedAndPersisted = new List<T>();
            this.updatedButNotPersisted = new List<T>();
        }

        internal void Create(T entity)
        {
            this.createdButNotPersisted.Add(entity);
        }

        internal void Update(T entity)
        {
            this.updatedButNotPersisted.Add(entity);
        }

        internal int Persist()
        {
            var persistanceCount = this.createdButNotPersisted.Count + this.updatedButNotPersisted.Count;
            if (this.createdButNotPersisted.Any())
            {
                this.CreatedAndPersisted.AddRange(this.createdButNotPersisted);
                this.createdButNotPersisted.Clear();
            }

            if (this.updatedButNotPersisted.Any())
            {
                this.updatedAndPersisted.AddRange(this.updatedButNotPersisted);
                this.updatedButNotPersisted.Clear();
            }

            return persistanceCount;
        }

        internal void AssertPersited()
        {
            Assert.Empty(this.createdButNotPersisted);
            Assert.Empty(this.updatedButNotPersisted);
            Assert.False(this.CreatedAndPersisted.Any() && this.updatedAndPersisted.Any(), $"The {typeof(T).Name} are not persisted.");
        }
    }
}
