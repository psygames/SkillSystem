using System.Collections.Generic;

namespace SkillSystem
{
    public class AbilityEvent : IEvent<AbilityAction>
    {
        public AbilityEventType name;
        public List<AbilityAction> actions;

        public Ability ability { get; private set; }
        public Modifier modifier { get; private set; }
        public bool isInModifier => modifier != null;

        public virtual Modifier GetModifier(string name)
        {
            return ability.GetModifier(name);
        }

        public void Reset(Ability ability, Modifier modifier = null)
        {
            this.ability = ability;
            this.modifier = modifier;

            if (actions != null)
            {
                foreach (var act in actions)
                {
                    act.Reset(this);
                }
            }
        }

        public void Merge(AbilityEvent evt)
        {
            if (actions != null && evt.actions != null)
            {
                actions.AddRange(evt.actions);
            }
            else if (actions == null)
            {
                actions = evt.actions;
            }
        }
    }
}
