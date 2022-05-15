using SkillSystem.Common;
using System.Collections.Generic;
namespace SkillSystem.Runtime
{
    public class AbilityRunner : BaseRunner
    {
        public FP castPoint { get; private set; }        // 前摇时长
        public FP backswing { get; private set; }        // 后摆时长
        public FP cooldown { get; private set; }         // 技能CD
        public FP duration { get; private set; }         // 技能释放僵直
        public FP channel { get; private set; }          // 持续施法时间
        public int castCount { get; set; } = -1;            // 可释放次数, -1 表示无限制

        public bool isCooldown => cooldown > 0;             // 冷却中
        public bool isCasting => castPoint > 0;             // 前摇中
        public bool isStiff => duration > 0;                // 硬直中
        public bool isChannelling => channel > 0;           // 持续施法中
        public bool isBackswing => backswing > 0;           // 后摇中
        public bool castable
        {
            get
            {
                return !isCooldown && !isStiff && castCount != 0;
            }
        }

        public Ability data { get; protected set; }
        public Ability originData { get; protected set; }

        public List<Ability> attachAbilities { get; private set; } = new List<Ability>();
        public List<Modifier> passiveModifiers { get; private set; }  // 被动技能BUFF

        public void Init(Boot boot, IUnit owner, Ability ability)
        {
            base.Init(boot, owner);
            originData = ability;
            this.data = ability;
            ResetData();
            Reset();
            ApplyPassiveModifier(ability);
        }

        public void Destroy()
        {
            foreach (var modi in passiveModifiers)
            {
                owner.modifierManager.Remove(modi.name);
            }
            passiveModifiers.Clear();
        }

        protected virtual void ApplyPassiveModifier(Ability ability)
        {
            if (passiveModifiers == null)
                passiveModifiers = new List<Modifier>();
            if (data.modifiers == null)
                return;
            foreach (var modi in data.modifiers)
            {
                if (!modi.isPassive)
                    continue;
                owner.modifierManager.Apply(owner, modi);
                passiveModifiers.Add(modi);
            }
        }

        protected virtual void RemovePassiveModifier(Ability ability)
        {
            if (passiveModifiers == null)
                passiveModifiers = new List<Modifier>();
            if (data.modifiers == null)
                return;
            foreach (var modi in data.modifiers)
            {
                if (!modi.isPassive)
                    continue;
                owner.modifierManager.Remove(modi.name);
                passiveModifiers.Remove(modi);
            }
        }

        public override void Reset()
        {
            base.Reset();

            castPoint = 0;
            cooldown = 0;
            duration = 0;
            channel = 0;
            backswing = 0;
        }

        public void ResetData()
        {
            ResetValues();
        }

        public void ResetValues()
        {
            // ref vals
            var refVals = GetReferenceValues();
            data.Reset(refVals);
            foreach (var ab in attachAbilities)
            {
                ab.Reset(refVals);
            }

            // custom vals merge
            var customVals = new List<NamedValue>();
            if (data.values != null)
            {
                foreach (var v in data.values)
                {
                    var index = customVals.FindIndex(a => a.name == v.name);
                    if (index >= 0)
                    {
                        customVals[index] = v;
                    }
                    else
                    {
                        customVals.Add(v);
                    }
                }
            }
            foreach (var ab in attachAbilities)
            {
                if (ab.values != null)
                {
                    foreach (var v in ab.values)
                    {
                        var index = customVals.FindIndex(a => a.name == v.name);
                        if (index >= 0)
                        {
                            customVals[index] = v;
                        }
                        else
                        {
                            customVals.Add(v);
                        }
                    }
                }
            }

            // custom vals set
            data.values = customVals;
            foreach (var ab in attachAbilities)
            {
                ab.values = customVals;
            }
        }

        public virtual void Cast()
        {
            // Spelling(Stiff) 中不能主动打断

            if (isCasting || isChannelling)
            {
                Break();
            }

            PhaseStart();
        }


        public override void Update(FP deltaTime)
        {
            base.Update(deltaTime);

            if (castPoint > 0)
            {
                castPoint = FMath.Max(0, castPoint - deltaTime);
                if (castPoint <= 0)
                {
                    SpellStart();
                }
            }

            if (duration > 0)
            {
                duration = FMath.Max(0, duration - deltaTime);
                if (duration <= 0)
                {
                    SpellFinish();
                }
            }

            if (channel > 0)
            {
                channel = FMath.Max(0, channel - deltaTime);
                if (channel <= 0)
                {
                    ChannelSucceed();
                }
            }

            if (backswing > 0)
            {
                backswing = FMath.Max(0, backswing - deltaTime);
                if (backswing <= 0)
                {
                    SpellOver();
                }
            }

            if (cooldown > 0)
            {
                cooldown = FMath.Max(0, cooldown - deltaTime);
                if (cooldown <= 0)
                {
                    CooldownOK();
                }
            }
        }

        //TODO: OPTIMIZE
        protected override List<AbilityEvent> GetEvents()
        {
            var evts = new List<AbilityEvent>();
            if (data.events != null)
            {
                evts.AddRange(data.events);
            }
            foreach (var at in attachAbilities)
            {
                if (at.events != null)
                {
                    evts.AddRange(at.events);
                }
            }
            return evts;
        }

        #region Inner API
        private void BreakCasting()
        {
            castPoint = 0;

            // 考虑添加 OnPhaseInterrupt 事件
        }

        private void BreakSpelling()
        {
            // 考虑添加 OnSpellInterrupt 事件
            duration = 0;
        }

        private void BreakChannelling()
        {
            channel = 0;
            ChannelInterrupt();
        }

        private void BreakBackswing()
        {
            backswing = 0;
        }

        private void CooldownOK()
        {
            // 技能 CD 完成，暂无事件需要处理
        }

        private void PhaseStart()
        {
            // 开始前摇
            castPoint = data.castPoint;
            TriggerEvent(AbilityEventType.OnAbilityPhaseStart);

            // 无前摇，直接释放技能
            if (castPoint <= 0)
            {
                SpellStart();
            }
        }


        // 开始硬直
        private void SpellStart()
        {
            // 消耗施法次数
            if (castCount > 0)
            {
                castCount -= 1;
            }

            // 冷却计时
            cooldown = data.cooldown;

            // 硬直计时
            duration = data.duration;

            // 开始持续施法
            if (data.behavior.HasFlag(BehaviorType.CHANNELLED))
            {
                channel = data.channelTime;
            }

            // 注意时序，要在 SpellFinish 之前
            TriggerEvent(AbilityEventType.OnSpellStart);

            // 没有硬直时间，直接硬直结束
            if (duration <= 0)
            {
                SpellFinish();
            }
        }

        // 硬直结束
        private void SpellFinish()
        {
            TriggerEvent(AbilityEventType.OnSpellFinish);
            backswing = data.baskswing;

            if (backswing <= 0)
            {
                SpellOver();
            }
        }

        // 后摆结束
        private void SpellOver()
        {
        }

        private void ChannelSucceed()
        {
            TriggerEvent(AbilityEventType.OnChannelSucceeded);
            ChannelFinish();
        }

        private void ChannelFinish()
        {
            TriggerEvent(AbilityEventType.OnChannelFinish);
        }

        private void ChannelInterrupt()
        {
            TriggerEvent(AbilityEventType.OnChannelInterrupted);
            ChannelFinish();
        }

        private List<ReferenceValue> GetReferenceValues()
        {
            return null;
            //var refVals = new List<ReferenceValue>();
            //foreach (var p in Enum.GetValues(typeof(PropertyType)))
            //{
            //    var pt = (PropertyType)p;
            //    var refVal = new ReferenceValue($"%{pt.ToString().ToLower()}", () => entity.property.Get(pt));
            //    refVals.Add(refVal);
            //}

            //return refVals;
        }
        #endregion

        #region Outer API
        public void Break()
        {
            if (isCasting)
            {
                BreakCasting();
            }

            if (isStiff)
            {
                BreakSpelling();
            }

            if (isChannelling)
            {
                BreakChannelling();
            }

            if (isBackswing)
            {
                BreakBackswing();
            }

            ClearDelayedAction();
        }

        public void Merge(Ability target)
        {
            if (target.attachType == AbilityAttachType.ORIGIN)
            {
                // Log.Error("不能替换为原始技能");
            }
            else if (target.attachType == AbilityAttachType.REPLACE)
            {
                RemovePassiveModifier(data);
                ApplyPassiveModifier(target);
                data = target;
                ResetData();
                Reset();
            }
            else if (target.attachType == AbilityAttachType.ATTACH)
            {
                attachAbilities.Add(target);
                ApplyPassiveModifier(target);
                ResetData();
                Reset();
            }
        }
        #endregion

    }
}
