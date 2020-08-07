// <copyright file="Entity.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core
{
    using System;

    public interface IAuditable
    {
        string CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        string LastModifiedBy { get; set; }

        DateTime LastModifiedDate { get; set; }
    }

    public interface IEntity : IEquatable<IEntity>, IAuditable
    {
        long Version { get; set; }

        bool IsValid();
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
            get { return !this.Id.Equals(default(TKey)); }
        }

        public bool Equals(IEntity other)
        {
            var entity = (Entity<TKey>)other;
            if (entity == null)
            {
                return false;
            }

            if (this.IsTransient)
            {
                return ReferenceEquals(this, entity);
            }

            return entity.Id.Equals(this.Id) && entity.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            if (this.IsTransient)
            {
                return base.GetHashCode();
            }

            return this.Id.GetHashCode();
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
