using SkillSystem.Common;
using System;

namespace SkillSystem
{
    public class ReferenceValue : NamedValue
    {
        public Func<float> value { get; set; }

        public ReferenceValue(string name, Func<float> value) : base(name)
        {
            this.value = value;
        }

        public override FP GetValue()
        {
            return value.Invoke();
        }
    }
}
