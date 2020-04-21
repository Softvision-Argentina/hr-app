using Core;
using System.Collections.Generic;

namespace Domain.Model
{
    public class SkillType: DescriptiveEntity<int>
    {
        public ICollection<Skill> Skills { get; set; }
    }
}
