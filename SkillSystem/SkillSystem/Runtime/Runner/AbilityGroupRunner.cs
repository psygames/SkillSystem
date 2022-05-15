using SkillSystem.Common;
using System.Collections.Generic;
namespace SkillSystem.Runtime
{
    public class AbilityGroupRunner
    {
        public IUnit owner { get; protected set; }

        public AbilityGroup data { get; private set; }

        public List<AbilityRunner> abilities { get; private set; } = new List<AbilityRunner>();

        public void Init(Boot boot, IUnit unit, AbilityGroup data)
        {
            this.owner = unit;
            this.data = data;
            foreach (var _data in data.abilities)
            {
                var runner = new AbilityRunner();
                runner.Init(boot, unit, _data);
                abilities.Add(runner);
            }
        }

        public void Destroy()
        {
            foreach (var ab in abilities)
            {
                ab.Destroy();
            }
        }

        protected void ResetChildrenData()
        {
            foreach (var ab in abilities)
            {
                ab.ResetData();
            }
        }

        public void Merge(AbilityGroup target)
        {
            foreach (var ab in abilities)
            {
                ab.Merge(target.abilities.Find(a => a.name == ab.data.name));
            }
        }


        public void Merge(Ability target)
        {
            foreach (var ab in abilities)
            {
                ab.Merge(target);
            }
        }


        public void Update(FP deltaTime)
        {
            foreach (var ab in abilities)
            {
                ab.Update(deltaTime);
            }
        }
    }
}
