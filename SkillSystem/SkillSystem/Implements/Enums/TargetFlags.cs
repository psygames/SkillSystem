namespace SkillSystem
{
    public enum TargetFlags
    {
        NONE = 0,
        Dead = 1 << 0,                  // 死亡的
        Invulnerable = 1 << 2,          // 无敌的
        ALL = -1,
    }
}
