using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public class TargetSelectorArea : TargetSelector
    {
        public AreaCenterType center = AreaCenterType.CASTER;
        public TargetType targetType = TargetType.TARGET;
        public TargetTeamType targetTeamType = TargetTeamType.ENEMY;
        public TargetUnitType targetUnitType = TargetUnitType.ALL;
        public TargetFlags targetFlags = TargetFlags.NONE;
        public int targetCount = 0;
        public FVector2 pivot = FVector2.Zero;
        public FP rotAngle = 0;
    }
}
