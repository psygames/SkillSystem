using SkillSystem.Common;
using System.Collections.Generic;

namespace SkillSystem.Runtime
{
    public class AbilityManager
    {
        protected List<AbilityRunner> abilities = new List<AbilityRunner>();
        protected List<AbilityGroupRunner> abilityGroups = new List<AbilityGroupRunner>();

        public Boot boot { get; protected set; }
        public IUnit owner { get; protected set; }
        public bool isPause { get; protected set; }
        public AbilityRunner current { get; protected set; }

        public virtual void Init(Boot boot, IUnit owner)
        {
            this.owner = owner;
        }

        public virtual void Destroy()
        {
            foreach (var ab in abilities)
            {
                ab.Destroy();
            }
            abilities.Clear();

            foreach (var abg in abilityGroups)
            {
                abg.Destroy();
            }
            abilityGroups.Clear();
        }

        public virtual AbilityRunner AddAbility(Ability ability)
        {
            var runner = new AbilityRunner();
            runner.Init(boot, owner, ability);
            abilities.Add(runner);
            AutoApplyModifier(ability);
            return runner;
        }

        public virtual AbilityGroupRunner AddAbilityGroup(AbilityGroup abilityGroup)
        {
            var runner = new AbilityGroupRunner();
            runner.Init(boot, owner, abilityGroup);
            abilityGroups.Add(runner);

            if (abilityGroup.abilities != null)
            {
                foreach (var ab in abilityGroup.abilities)
                    AutoApplyModifier(ab);
            }

            return runner;
        }

        protected virtual void AutoApplyModifier(Ability ability)
        {
            if (ability.modifiers == null)
                return;
            foreach (var modi in ability.modifiers)
            {
                if (!modi.isPassive)
                    continue;
                owner.modifierManager.Apply(owner, modi);
            }
        }

        public virtual void Pause()
        {
            isPause = true;
        }

        public virtual void Resume()
        {
            isPause = false;
        }

        public virtual bool Cast(string abilityName, TargetWrapper targetWrapper = null)
        {
            var ability = abilities.Find(a => a.data.name == abilityName);
            if (ability == null)
            {
                Log.Error($"Can not find ability: {abilityName}");
            }
            return Cast(ability, targetWrapper);
        }

        public virtual bool Cast(AbilityRunner ability, TargetWrapper targetWrapper = null)
        {
            if (!CanCast(ability))
            {
                return false;
            }

            // 打断当前技能
            if (!ability.data.behavior.HasFlag(BehaviorType.IMMEDIATE)
                && current != null)
            {
                current.Break();
            }

            ability.SetTarget(targetWrapper);
            ability.Cast();
            current = ability;

            // 触发攻击事件
            if (ability.data.behavior.HasFlag(BehaviorType.ATTACK))
            {
                TriggerOwnerEvent(AbilityEventType.OnOwnerAttackStart);
                owner.modifierManager.OnAttack();
            }
            return true;
        }

        public bool CanCast(AbilityRunner ability)
        {
            if (!ability.castable)
                return false;
            // 立即释放类型技能
            if (ability.data.behavior.HasFlag(BehaviorType.IMMEDIATE))
                return true;
            if (current == null)
                return true;
            // 可打断当前技能后摇
            if (current.isBackswing && ability.data.priority >= current.data.priority)
                return true;
            return false;
        }

        public virtual void Update(FP deltaTime)
        {
            if (isPause)
                return;

            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                if (i >= abilities.Count)
                    continue;
                abilities[i].Update(deltaTime);
            }

            for (int i = abilityGroups.Count - 1; i >= 0; i--)
            {
                if (i >= abilityGroups.Count)
                    continue;
                abilityGroups[i].Update(deltaTime);
            }

            // 技能完成
            if (current != null && !current.isCasting
                && !current.isStiff && !current.isChannelling
                && !current.isBackswing)
            {
                current = null;
            }
        }

        public AbilityRunner GetAbility(string name)
        {
            foreach (var ab in abilities)
            {
                if (ab.data.name == name)
                    return ab;
            }
            return null;
        }

        public AbilityRunner GetAbility(int index)
        {
            if (index >= abilities.Count)
                return null;
            return abilities[index];
        }

        public AbilityGroupRunner GetAbilityGroup(string name)
        {
            foreach (var ab in abilityGroups)
            {
                if (ab.data.name == name)
                    return ab;
            }
            return null;
        }

        public List<AbilityRunner> GetAllAbilities()
        {
            return abilities;
        }

        public virtual void TriggerOwnerEvent(AbilityEventType _event, TargetWrapper target = null)
        {
            foreach (var abg in abilityGroups)
            {
                foreach (var ab in abg.abilities)
                {
                    ab.TriggerEvent(_event, target);
                }
            }

            foreach (var ab in abilities)
            {
                ab.TriggerEvent(_event, target);
            }
        }
    }
}
