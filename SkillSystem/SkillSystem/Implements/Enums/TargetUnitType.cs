namespace SkillSystem
{
    public enum TargetUnitType
    {
        NONE = 0,
        BASIC = 1 << 0,
        HERO = 1 << 1,
        BUILDING = 1 << 2,
        ALL = -1,
    }
}
