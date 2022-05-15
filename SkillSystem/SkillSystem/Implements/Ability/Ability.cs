using SkillSystem.Common;
using System.Collections.Generic;

namespace SkillSystem
{
    public class Ability
    {
        public string name;
        public string image;                                            // 图标
        public AbilityPriority priority = AbilityPriority.Low;          // 优先级
        public BehaviorType behavior = BehaviorType.ATTACK;             // 技能类型
        public DamageType damageType = DamageType.PHYSICAL;             // 技能伤害类型
        public TargetTeamType targetTeamType = TargetTeamType.ENEMY;    // 目标队伍类型
        public TargetUnitType targetUnitType = TargetUnitType.ALL;      // 目标单位类型
        public NoNameExpressionValue castPoint;                         // 前摇时长
        public NoNameExpressionValue baskswing;                         // 后摇时长
        public NoNameExpressionValue castRange;                         // 施法距离
        public NoNameExpressionValue duration;                          // 施法硬直
        public NoNameExpressionValue cooldown;                          // 技能冷却
        public NoNameExpressionValue channelTime;                       // 持续施法时长，可被打断
        public AbilityAttachType attachType = AbilityAttachType.ORIGIN; // 技能附加类型

        public List<AbilityEvent> events;
        public List<Modifier> modifiers;
        public List<NamedValue> values;
        public List<ReferenceValue> referenceValues { get; set; }

        public void Reset(List<ReferenceValue> referenceValues)
        {
            if (events != null)
            {
                foreach (var evt in events)
                {
                    evt.Reset(this);
                }
            }

            if (modifiers != null)
            {
                foreach (var modifier in modifiers)
                {
                    modifier.Reset(this);
                }
            }

            this.referenceValues = referenceValues;

            ApplyValue(this);
        }

        public Modifier GetModifier(string name)
        {
            if (modifiers != null)
            {
                foreach (var m in modifiers)
                {
                    if (m.name == name)
                        return m;
                }
            }
            return null;
        }

        public void Merge(Ability ability)
        {
            if (modifiers != null && ability.modifiers != null)
            {
                foreach (var m in ability.modifiers)
                {
                    var sameModi = modifiers.Find(a => a.name == m.name);
                    if (sameModi != null)
                    {
                        sameModi.Merge(m);
                    }
                    else
                    {
                        modifiers.Add(m);
                    }
                }
            }
            else if (modifiers == null)
            {
                modifiers = ability.modifiers;
            }

            if (events != null && ability.events != null)
            {
                foreach (var evt in ability.events)
                {
                    var sameEvt = events.Find(a => a.name == evt.name);
                    if (sameEvt != null)
                    {
                        sameEvt.Merge(evt);
                    }
                    else
                    {
                        events.Add(evt);
                    }
                }
            }
            else if (events == null)
            {
                events = ability.events;
            }
        }

        public void ApplyValue(object obj)
        {
            foreach (var field in obj.GetType().GetFields())
            {
                if (typeof(NoNameExpressionValue).IsAssignableFrom(field.FieldType))
                {
                    var _obj = field.GetValue(obj);
                    if (_obj != null)
                    {
                        _obj.GetType().GetMethod("Reset").Invoke(_obj, new object[] { this });
                    }
                }
            }
            if (values != null)
            {
                foreach (var val in values)
                {
                    val.Reset(this);
                }
            }
        }

        public bool SetValue(string name, FP val)
        {
            if (values == null)
                return false;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].name == name)
                {
                    values[i] = new FloatValue(name, val);
                    return true;
                }
            }
            return false;
        }

        public static Ability Load(string name)
        {
            Log.Info($"Load Ability: {name}");
            //var bytes = ResUtil.Load<TextAsset>($"AbilitySystem/{name}").bytes;
            //var ability = Serializer.Deserialize(bytes, typeof(Ability));
            ////TODO: REMOVE CODE HERE
            //var table = TablePlayerAbility.Get(name);
            //if (table != null && !string.IsNullOrEmpty(table.icon))
            //{
            //    ((Ability)ability).image = table.icon;
            //}
            //return (Ability)ability;
            return null;
        }
    }
}
