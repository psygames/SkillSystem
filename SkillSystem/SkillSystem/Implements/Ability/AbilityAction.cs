using System.Collections.Generic;

namespace SkillSystem
{
    public class AbilityAction : IAction
    {
        public TargetSelector targetSelector;

        public AbilityEvent owner { get; private set; }
        public Ability ability => owner.ability;

        public virtual void Reset(AbilityEvent owner)
        {
            this.owner = owner;
            ability.ApplyValue(this);

            if (targetSelector != null)
            {
                targetSelector.Reset(this);
            }
        }

        public virtual void ApplyTempValues(List<NamedValue> values)
        {
            foreach (var field in this.GetType().GetFields())
            {
                if (typeof(NoNameExpressionValue).IsAssignableFrom(field.FieldType))
                {
                    var _obj = field.GetValue(this);
                    if (_obj != null)
                    {
                        _obj.GetType().GetMethod("SetTempValues").Invoke(_obj, new object[] { values });
                    }
                }
            }
        }
    }
}
