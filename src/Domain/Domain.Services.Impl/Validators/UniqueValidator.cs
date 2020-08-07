// <copyright file="UniqueValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core;
    using FluentValidation;
    using FluentValidation.Validators;

    public class UniquePropertyValidator<T, TProperty> : PropertyValidator where T : IEntity
    {
        private readonly Func<IQueryable<T>> collectionAccessorFunc;

        public UniquePropertyValidator(Func<IQueryable<T>> collectionAccessorFunc)
            : base("The {PropertyName} must be unique.")
        {
            this.collectionAccessorFunc = collectionAccessorFunc;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var entities = this.collectionAccessorFunc();
            Expression<Func<T, bool>> predicate = GetPropertyPredicate(context);

            var match = entities.Count(predicate);
            return match == 0;
        }

        private static Expression<Func<T, bool>> GetPropertyPredicate(PropertyValidatorContext context)
        {
            var propertyValue = context.PropertyValue;
            var arg = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(arg, context.PropertyName);

            var expression = Expression.Equal(
                property,
                Expression.Constant(propertyValue));

            var predicate = Expression.Lambda<Func<T, bool>>(expression, arg);

            return predicate;
        }
    }

    public static class UniqueExtension
    {
        public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, Func<IQueryable<T>> collection) where T : IEntity
        {
            return ruleBuilder.SetValidator(new UniquePropertyValidator<T, TProperty>(collection));
        }
    }
}
