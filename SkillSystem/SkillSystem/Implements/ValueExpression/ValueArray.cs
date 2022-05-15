using SkillSystem.Common;
using System.Collections.Generic;

namespace SkillSystem
{
    public class ValueArray : NamedValue
    {
        public NoNameExpressionValue index;
        public List<NoNameExpressionValue> values;
        public ValueArray() { }
        public ValueArray(string name, List<NoNameExpressionValue> values) : base(name)
        {
            this.values = values;
        }

        public override FP GetValue()
        {
            return values[(int)index.GetValue()];
        }
    }
}
