using System.Collections.Generic;

namespace SkillSystem
{
    public class Modifier
    {
        public string name;
        public string image;

        public bool isPassive;              // 被动Modifier自动生效，无时长限制
        public bool isHidden;               // 不显示在HUD上

        public bool isDebuff;               // 减益BUFF
        public bool isStackable;            // 可叠加

        public NoNameExpressionValue duration;      // 持续时间
        public NoNameExpressionValue attackCount;   // 持续攻击次数
        public NoNameExpressionValue timerInterval; // 创建计时器时间，0表示无计时器

        public List<AbilityEvent> events;
        public List<ModifierProperty> properties;
        public List<ModifierState> states;

        public Ability owner { get; private set; }

        public void Reset(Ability ability)
        {
            this.owner = ability;

            if (events != null)
            {
                foreach (var evt in events)
                {
                    evt.Reset(ability, this);
                }
            }

            ability.ApplyValue(this);
            if (properties != null)
            {
                foreach (var p in properties)
                {
                    p.value.Reset(ability);
                }
            }
        }

        public static Modifier Load(string name)
        {
            //Log.Debug($"Load Modifier : {name}");
            //var bytes = ResUtil.Load<TextAsset>($"AbilitySystem/Modifier/{name}").bytes;
            //var modifier = Serializer.Deserialize(bytes, typeof(Modifier));
            //return (Modifier)modifier;
            return null;
        }

        public void Merge(Modifier modifier)
        {
            if (events != null && modifier.events != null)
            {
                foreach (var evt in modifier.events)
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
                events = modifier.events;
            }

            if (properties != null && modifier.properties != null)
            {
                properties.AddRange(modifier.properties);
            }
            else if (properties == null)
            {
                properties = modifier.properties;
            }

            if (states != null && modifier.states != null)
            {
                states.AddRange(modifier.states);
            }
            else if (states == null)
            {
                states = modifier.states;
            }
        }
    }
}
