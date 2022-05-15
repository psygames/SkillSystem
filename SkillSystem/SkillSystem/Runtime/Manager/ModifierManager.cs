using SkillSystem.Common;
using System.Collections.Generic;
namespace SkillSystem.Runtime
{
    public class ModifierManager
    {
        protected List<ModifierRunner> modifiers = new List<ModifierRunner>();

        public Boot boot { get; protected set; }
        public IUnit owner { get; protected set; }
        public bool isPause { get; protected set; }

        public virtual void Init(Boot boot, IUnit owner)
        {
            this.owner = owner;
            this.boot = boot;
            isRemoveAll = false;
        }

        public virtual void Pause()
        {
            isPause = true;
        }

        public virtual void Resume()
        {
            isPause = false;
        }

        public virtual void Apply(IUnit caster, Modifier modifier, TargetWrapper target = null)
        {
            ModifierRunner runner = null;
            if (!modifier.isStackable && modifiers.Find(a => a.data == modifier) != null)
            {
                runner = modifiers.Find(a => a.data == modifier);
                runner.SetTarget(target);
                runner.Restart();
            }
            else
            {
                runner = new ModifierRunner();
                runner.Init(boot, caster, owner, modifier);
                modifiers.Add(runner);
                runner.SetTarget(target);
                runner.Apply();
            }
        }

        // TODO: 考虑名字唯一问题
        public virtual void Remove(string name)
        {
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (modifiers[i].data.name == name)
                {
                    modifiers[i].Unapply();
                    modifiers.RemoveAt(i);
                }
            }
        }


        private bool isRemoveAll = false;
        public virtual void RemoveAll()
        {
            isRemoveAll = true;
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (i >= modifiers.Count)
                    continue;
                modifiers[i].Unapply();
            }

            modifiers.Clear();
        }

        public virtual void Update(FP deltaTime)
        {
            if (isPause)
                return;

            // 有可能在 Update 的时候，触发事件导致 Modifier 被移除或被清空。
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (i >= modifiers.Count)
                    continue;
                modifiers[i].Update(deltaTime);
            }

            // 缓存移除状态
            if (isRemoveAll)
            {
                //TODO: Cache Remove
            }

            // 检测失效 Modifier
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (!modifiers[i].isActive)
                {
                    modifiers.RemoveAt(i);
                }
            }
        }

        public bool HasModifier(string name)
        {
            return GetModifier(name) != null;
        }

        public ModifierRunner GetModifier(string name)
        {
            foreach (var modi in modifiers)
            {
                if (modi.data.name == name)
                    return modi;
            }
            return null;
        }

        public List<ModifierRunner> GetModifiers()
        {
            return modifiers;
        }

        public void ApplyAllModifierProperties()
        {
            foreach (var modi in modifiers)
            {
                if (modi.isActive)
                {
                    modi.ApplyProperties();
                }
            }
        }

        public virtual void OnAttack()
        {
            foreach (var modi in modifiers)
            {
                if (modi.isActive)
                {
                    modi.OnAttack();
                }
            }
        }

        public virtual void TriggerOwnerEvent(AbilityEventType _event, TargetWrapper target = null)
        {
            foreach (var modi in modifiers)
            {
                if (modi.isActive)
                {
                    if (target == null)
                        modi.TriggerEvent(_event, modi.target);
                    else
                        modi.TriggerEvent(_event, target);
                }
            }
        }
    }
}
