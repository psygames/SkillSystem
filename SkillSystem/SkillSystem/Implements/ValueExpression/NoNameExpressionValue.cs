using System;

namespace SkillSystem
{
    public class NoNameExpressionValue : ExpressionValue
    {
        [NonSerialized]
        private new string name;
        public NoNameExpressionValue(string value) : base(null, value) { }

        public override string ToString()
        {
            return value;
        }

        public static NoNameExpressionValue Parse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            if (str == "0")
                return null;
            return new NoNameExpressionValue(str);
        }

        public static implicit operator NoNameExpressionValue(string value)
        {
            return new NoNameExpressionValue(value);
        }
    }
}
