namespace SkillSystem
{
    public enum TargetTeamType
    {
        NONE = 0,
        ENEMY = 1 << 0,
        FRIEND = 1 << 1,
        NEUTRAL = 1 << 3,
        ALL = -1,
    }
}
