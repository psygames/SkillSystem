using System.Collections.Generic;
using SkillSystem.Common;
namespace SkillSystem.Runtime
{
    public class ModifierRunner : BaseRunner
    {
        public IUnit caster { get; protected set; }     // 施法者，区别于拥有者
        public Modifier data { get; protected set; }

        public bool isActive { get; protected set; }
        public FP cooldown { get; protected set; }
        public FP attackCount { get; protected set; }
        public FP intervalCD { get; protected set; }

        public void Init(Boot boot, IUnit caster, IUnit owner, Modifier modifier)
        {
            this.caster = caster;
            this.data = modifier;
            base.Init(boot, owner);
            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            intervalCD = 0;
            cooldown = 0;
            isActive = false;
        }

        public void Apply()
        {
            if (isActive)
                return;
            Reset();
            isActive = true;
            cooldown = data.duration;
            attackCount = data.attackCount;
            ApplyProperties();
            ApplyStates();
            OnCreated();
        }

        public void Restart()
        {
            Unapply();
            Apply();
        }

        public void Unapply()
        {
            if (!isActive)
                return;
            isActive = false;
            ClearDelayedAction();
            UnapplyProperties();
            UnapplyStates();
            OnDestroy();
        }

        private void ApplyState(ModifierState state)
        {
            owner.ApplyModifierState(state);
        }

        private void ApplyStates()
        {
            if (data.states != null)
            {
                foreach (var s in data.states)
                {
                    ApplyState(s);
                }
            }
        }

        private void UnapplyState(ModifierState state)
        {
            owner.UnapplyModifierState(state);
        }

        private void UnapplyStates()
        {
            if (data.states != null)
            {
                foreach (var s in data.states)
                {
                    UnapplyState(s);
                }
            }
        }

        private void ApplyProperty(ModifierProperty p)
        {
            owner.ApplyProperty(p);
        }

        public void ApplyProperties()
        {
            if (data.properties != null)
            {
                foreach (var p in data.properties)
                {
                    ApplyProperty(p);
                }
            }
        }

        private void UnapplyProperty(ModifierProperty p)
        {
            owner.UnapplyProperty(p);
        }

        private void UnapplyProperties()
        {
            if (data.properties != null)
            {
                foreach (var p in data.properties)
                {
                    UnapplyProperty(p);
                }
            }
        }

        public override void Update(FP deltaTime)
        {
            base.Update(deltaTime);

            if (!isActive)
                return;

            if (!data.isPassive && data.duration != FP.Zero)
            {
                cooldown = FMath.Max(cooldown - deltaTime, 0);

                if (cooldown == FP.Zero)
                {
                    Unapply();
                }
            }

            //TODO: 一帧触发多次情况，interval 残余值
            if (data.timerInterval != FP.Zero)
            {
                intervalCD = FMath.Max(intervalCD - deltaTime, 0);

                if ( intervalCD == FP.Zero)
                {
                    intervalCD = intervalCD + data.timerInterval;
                    OnTimerInterval();
                }
            }
        }

        private void OnCreated()
        {
            TriggerEvent(AbilityEventType.OnCreated);
        }

        private void OnTimerInterval()
        {
            TriggerEvent(AbilityEventType.OnTimerInterval);
        }

        private void OnDestroy()
        {
            TriggerEvent(AbilityEventType.OnDestroy);
        }

        public void OnAttack()
        {
            if (data.attackCount != FP.Zero && attackCount > 0)
            {
                attackCount -= 1;
                if (attackCount == 0)
                {
                    Unapply();
                }
            }
        }

        protected override List<AbilityEvent> GetEvents()
        {
            return data.events;
        }
    }
}
