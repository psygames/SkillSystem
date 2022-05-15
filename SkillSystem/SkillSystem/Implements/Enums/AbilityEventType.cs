namespace SkillSystem
{
    public enum AbilityEventType
    {
        None = 0,                                   // 无用，占位
        // ---- 当前技能触发 ----//
        OnAbilityPhaseStart = 1,                    // 开始施法（在施法前摇前）
        OnSpellStart = 2,                           // 开始释放技能（在施法前摇后）
        OnSpellFinish = 3,                          // 完成释放技能
        OnAnimationImpact = 4,                      // 动画攻击判定
        OnAttackHit = 5,                            // 攻击击中目标
        OnPhysicsHit = 6,                           // 物理碰撞
        OnMakeDamage = 10,                          // 造成伤害
        OnChannelInterrupted = 7,                   // 持续施法被打断 
        OnChannelSucceeded = 8,                     // 持续施法完成，并且没有被打断
        OnChannelFinish = 9,                        // 持续施法完成

        // ---- 当前技能投掷物触发 ----//
        OnProjectileHit = 101,                      // 投掷物击中目标
        OnProjectileHitPoint = 102,                 // 投掷物击中点
        OnProjectileFinish = 103,                   // 投掷物完成


        // ---- 当前Modifier触发 ----//
        OnCreated = 201,                            // 创建       
        OnDestroy = 202,                            // 销毁 
        OnTimerInterval = 203,                      // 计时器触发

        // ---- 拥有者触发 ----/
        OnOwnerDied = 301,                          // 自己死亡
        OnOwnerTakeDamage = 302,                    // 自己受伤
        OnOwnerMakeDamage = 304,                    // 自己造成伤害
        OnOwnerAttackOrder = 305,                   // 自己开始攻击行为
        OnOwnerAttackStart = 306,                   // 自己开始攻击行为
        OnOwnerHpChanged = 309,                     // 自己血量改变
        OnOwnerDodged = 310,                        // 自己闪避攻击

        // ---- 任意单位触发 ----//                   // TODO: 需要一个范围值
        OnAnyAbilityExecuted = 401,                 // 任意技能执行（当SpellStart时）
        OnAnyDeath = 402,                           // 任意指令输入
    }
}
