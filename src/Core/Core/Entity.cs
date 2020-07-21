using System;

namespace Core
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string LastModifiedBy { get; set; }
        DateTime LastModifiedDate { get; set; }
    }

    public interface IEntity : IEquatable<IEntity>, IAuditable
    {
        bool IsValid();
        long Version { get; set; }
    }

    public abstract class Entity<TKey> : IEntity where TKey : IComparable, IFormattable
    {
        public TKey Id { get; set; }
        public long Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        private bool IsTransient
        {
            get { return !Id.Equals(default(TKey)); }
        }

        public bool Equals(IEntity other)
        {
            var entity = (Entity<TKey>)other;
            if (entity == null) return false;
            if (IsTransient) return ReferenceEquals(this, entity);
            return entity.Id.Equals(Id) && entity.GetType() == GetType();
        }

        public override int GetHashCode()
        {
            if (IsTransient) return base.GetHashCode();
            return Id.GetHashCode();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class DescriptiveEntity<TKey> : Entity<TKey> where TKey : IComparable, IFormattable
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
