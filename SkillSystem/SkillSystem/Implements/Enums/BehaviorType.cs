namespace SkillSystem
{
    public enum BehaviorType
    {
        NONE = 0,
        /// <summary>
        /// 隐藏技能，不能释放，也不会显示在HUD上。
        /// </summary>
        HIDDEN = 1 << 0,
        /// <summary>
        /// 被动技能，不能释放，但会显示在HUD上。
        /// </summary>
        PASSIVE = 1 << 1,
        /// <summary>
        /// 无需目标
        /// </summary>
        NO_TARGET = 1 << 2,
        /// <summary>
        /// 需要目标
        /// </summary>
        UNIT_TARGET = 1 << 3,
        /// <summary>
        /// 需要指定位置
        /// </summary>
        POINT = 1 << 4,
        /// <summary>
        /// 需要指定范围
        /// </summary>
        AOE = 1 << 5,
        /// <summary>
        /// 持续施法
        /// </summary>
        CHANNELLED = 1 << 7,
        /// <summary>
        /// 具有英雄的方向
        /// </summary>
        DIRECTIONAL = 1 << 10,
        /// <summary>
        /// 立即执行，无动作
        /// </summary>
        IMMEDIATE = 1 << 11,
        /// <summary>
        /// 自动释放
        /// </summary>
        AUTOCAST = 1 << 12,
        /// <summary>
        /// 普攻类型技能
        /// </summary>
        ATTACK = 1 << 15,
        /// <summary>
        /// 不能被打断
        /// </summary>
        CANT_BREAK = 1 << 16,
        ALL = -1,
    }
}
