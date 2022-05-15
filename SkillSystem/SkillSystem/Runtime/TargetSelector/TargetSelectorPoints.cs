using System.Collections.Generic;
using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public class TargetSelectorPoints : TargetSelector
    {
        public AreaCenterType center = AreaCenterType.CASTER;
        public List<FVector3> points;
    }
}
