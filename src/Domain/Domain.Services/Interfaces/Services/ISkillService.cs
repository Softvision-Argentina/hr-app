// <copyright file="ISkillService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Skill;

    public interface ISkillService
    {
        CreatedSkillContract Create(CreateSkillContract contract);

        ReadedSkillContract Read(int id);

        void Update(UpdateSkillContract contract);

        void Delete(int id);

        IEnumerable<ReadedSkillContract> List();
    }
}
