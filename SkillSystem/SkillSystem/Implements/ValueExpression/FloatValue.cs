using SkillSystem.Common;

namespace SkillSystem
{
    public class FloatValue : NamedValue
    {
        public FP value;
        public FloatValue() { }
        public FloatValue(string name, FP val) : base(name)
        {
            this.value = val;
        }

        public override FP GetValue()
        {
            return value;
        }
    }
}
