﻿// <copyright file="ITestingHelpers.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Domain.Services.Repositories.EF;

    public interface ITestingHelpers
    {
        void ContextAction(Action<DataBaseContext> action);

        void UseService<T>(Action<T> action) where T : class;

        void Seed<T>(List<T> entities) where T : class;

        void Seed<T>(T entity) where T : class;

        T Get<T>(int id) where T : Entity<int>;

        int GetCount<T>() where T : class;

        T GetEager<T>(int id) where T : class;

        // void CleanTestingDatabase();
    }
}
