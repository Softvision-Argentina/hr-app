// <copyright file="ISkillTypeService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.SkillType;

    public interface ISkillTypeService
    {
        CreatedSkillTypeContract Create(CreateSkillTypeContract contract);

        ReadedSkillTypeContract Read(int id);

        void Update(UpdateSkillTypeContract contract);

        void Delete(int id);

        IEnumerable<ReadedSkillTypeContract> List();
    }
}
