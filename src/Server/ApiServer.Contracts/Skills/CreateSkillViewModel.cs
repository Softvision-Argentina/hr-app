// <copyright file="CreateSkillViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Skills
{
    public class CreateSkillViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }
    }
}
