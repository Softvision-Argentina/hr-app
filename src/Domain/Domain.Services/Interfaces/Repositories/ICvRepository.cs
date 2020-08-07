// <copyright file="ICvRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Domain.Model;

    public interface ICvRepository
    {
        Cv GetCv(int id);

        bool SaveAll(Cv cv);
    }
}
