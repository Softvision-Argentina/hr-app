// <copyright file="IBuilder.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
