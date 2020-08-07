// <copyright file="ReadedSkillContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Skill
{
    public class ReadedSkillContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }
    }
}
