// <copyright file="IMigrator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Persistance
{
    using DependencyInjection.Config;

    public interface IMigrator
    {
        void Migrate(DatabaseConfigurations dbConfig);
    }
}
