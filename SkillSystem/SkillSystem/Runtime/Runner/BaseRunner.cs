using System;
using System.Collections.Generic;
using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public class BaseRunner
    {
        public Boot boot { get; private set; }
        public virtual IUnit owner { get; private set; }
        public virtual TargetWrapper target { get; private set; }

        private List<DelayedActionRunner> delayedActions = new List<DelayedActionRunner>();

        protected virtual void Init(Boot boot, IUnit owner)
        {
            this.boot = boot;
            this.owner = owner;
        }

        public virtual void Reset()
        {
            ClearDelayedAction();
        }

        public virtual void RunDelayedAction(FP delay, Action callback)
        {
            delayedActions.Add(DelayedActionRunner.Get(delay, callback));
        }

        public virtual void ClearDelayedAction()
        {
            for (int i = delayedActions.Count - 1; i >= 0 && i < delayedActions.Count; i--)
            {
                delayedActions[i].Release();
                delayedActions.RemoveAt(i);
            }
        }

        public virtual void Update(FP deltaTime)
        {
            for (int i = delayedActions.Count - 1; i >= 0 && i < delayedActions.Count; i--)
            {
                if (delayedActions[i].isExpired)
                {
                    delayedActions[i].Release();
                    delayedActions.RemoveAt(i);
                }
                else
                {
                    delayedActions[i].Update(deltaTime);
                }
            }
        }


        #region Outer APIs
        public virtual void SetTarget(TargetWrapper wrapper)
        {
            target = wrapper;
        }

        public virtual void TriggerEvent(AbilityEventType evt, List<NamedValue> tempValues = null)
        {
            if (target == null)
                TriggerEvent(evt, owner, tempValues);
            else
                TriggerEvent(evt, target, tempValues);
        }

        public virtual void TriggerEvent(AbilityEventType evt, FVector3 point, List<NamedValue> tempValues = null)
        {
            TriggerEvent(evt, TargetWrapper.Get(point), tempValues);
        }

        public virtual void TriggerEvent(AbilityEventType evt, IUnit unit, List<NamedValue> tempValues = null)
        {
            TriggerEvent(evt, TargetWrapper.Get(unit), tempValues);
        }

        public virtual void TriggerEvent(AbilityEventType evt, TargetWrapper target, List<NamedValue> tempValues = null)
        {
            HandleEvents(GetEvents(), evt, target, tempValues);
        }

        #endregion

        #region Inner APIs

        protected virtual List<AbilityEvent> GetEvents()
        {
            throw new Exception("Not Override GetEvents");
        }

        protected virtual void HandleEvents(List<AbilityEvent> events, AbilityEventType evt, TargetWrapper target, List<NamedValue> tempValues = null)
        {
            if (events != null)
            {
                foreach (var _evt in events)
                {
                    if (_evt.name == evt)
                    {
                        HandleActions(_evt.actions, target, tempValues);
                    }
                }
            }
        }

        protected void HandleActions(IList<AbilityAction> actions, TargetWrapper target, List<NamedValue> tempValues = null)
        {
            if (actions != null)
            {
                foreach (var act in actions)
                {
                    var actionWrapper = ActionWrapper.Get(this, act, target, tempValues);
                    boot.actionHandler.Handle(actionWrapper, act);
                }
            }
        }
        #endregion
    }
}
