using Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests
{
    public class MockPersistance<T> where T : IEntity
    {
        List<T> _createdButNotPersisted;
        List<T> _updatedButNotPersisted;
        List<T> _updatedAndPersisted;

        internal int PersistanceCount => CreatedAndPersisted.Count + _updatedAndPersisted.Count;

        public List<T> CreatedAndPersisted { get; set; }

        internal MockPersistance()
        {
            CreatedAndPersisted = new List<T>();
            _createdButNotPersisted = new List<T>();
            _updatedAndPersisted = new List<T>();
            _updatedButNotPersisted = new List<T>();
        }

        internal void Create(T entity)
        {
            _createdButNotPersisted.Add(entity);
        }

        internal void Update(T entity)
        {
            _updatedButNotPersisted.Add(entity);
        }

        internal int Persist()
        {
            var persistanceCount = _createdButNotPersisted.Count + _updatedButNotPersisted.Count;
            if (_createdButNotPersisted.Any())
            {
                CreatedAndPersisted.AddRange(_createdButNotPersisted);
                _createdButNotPersisted.Clear();
            }

            if (_updatedButNotPersisted.Any())
            {
                _updatedAndPersisted.AddRange(_updatedButNotPersisted);
                _updatedButNotPersisted.Clear();
            }
            return persistanceCount;
        }

        internal void AssertPersited()
        {
            Assert.Empty(_createdButNotPersisted);
            Assert.Empty(_updatedButNotPersisted);
            Assert.False(CreatedAndPersisted.Any() && _updatedAndPersisted.Any(), $"The {typeof(T).Name} are not persisted.");
        }

    }
}
