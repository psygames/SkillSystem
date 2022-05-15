using System.Collections.Generic;
using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public partial class ActionWrapper
    {
        public BaseRunner runner { get; private set; }
        public AbilityAction action { get; private set; }
        public TargetWrapper target { get; private set; }

        public bool isModifierAction => runner is ModifierRunner;
        public IUnit owner => isModifierAction ? modifier.owner : ability.owner;
        public IUnit caster => isModifierAction ? modifier.caster : ability.owner;
        public AbilityRunner ability => isModifierAction ? null : runner as AbilityRunner;
        public ModifierRunner modifier => isModifierAction ? runner as ModifierRunner : null;

        #region APIs
        public List<TargetWrapper> GetTargets()
        {
            return GetTargets(target, action.targetSelector);
        }

        public void TriggerEvent(AbilityEventType evt, IUnit unit, FloatValue value)
        {
            TriggerEvent(evt, unit, new List<NamedValue>() { value });
        }

        public void TriggerEvent(AbilityEventType evt, FVector3 point, FloatValue value)
        {
            TriggerEvent(evt, point, new List<NamedValue>() { value });
        }

        public void TriggerEvent(AbilityEventType evt, TargetWrapper target, FloatValue value)
        {
            TriggerEvent(evt, target, new List<NamedValue>() { value });
        }

        public void TriggerEvent(AbilityEventType evt, IUnit unit, List<NamedValue> tempValues = null)
        {
            runner.TriggerEvent(evt, unit, tempValues);
        }

        public void TriggerEvent(AbilityEventType evt, FVector3 point, List<NamedValue> tempValues = null)
        {
            runner.TriggerEvent(evt, point, tempValues);
        }

        public void TriggerEvent(AbilityEventType evt, TargetWrapper target, List<NamedValue> tempValues = null)
        {
            runner.TriggerEvent(evt, target, tempValues);
        }

        public static ActionWrapper Get(BaseRunner runner, AbilityAction action, TargetWrapper target, List<NamedValue> tempValues = null)
        {
            var wrapper = new ActionWrapper();
            wrapper.runner = runner;
            wrapper.action = action;
            wrapper.target = target;
            wrapper.action.ApplyTempValues(tempValues);
            return wrapper;
        }
        #endregion
    }
}
