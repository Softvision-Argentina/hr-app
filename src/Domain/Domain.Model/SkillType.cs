// <copyright file="SkillType.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class SkillType : DescriptiveEntity<int>
    {
        public ICollection<Skill> Skills { get; set; }
    }
}
