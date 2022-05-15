using SkillSystem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkillSystem
{
    public class EvalValue : Value
    {
        public Value value1;
        public Value value2;
        public EvalMethod method;

        public EvalValue() { }

        public EvalValue(Value val1, Value val2, EvalMethod method)
        {
            this.value1 = val1;
            this.value2 = val2;
            this.method = method;
        }

        private char MethodToChar(EvalMethod method)
        {
            if (method == EvalMethod.Add) return '+';
            if (method == EvalMethod.Subtract) return '-';
            if (method == EvalMethod.Multiply) return '*';
            if (method == EvalMethod.Divide) return '/';
            return 'E';
        }

        public override FP GetValue()
        {
#if UNITY_EDITOR
            // UnityEngine.Debug.Log($"Eval: {value1.GetValue()} <color=yellow>{MethodToChar(method)}</color> {value2.GetValue()}");
#endif

            if (method == EvalMethod.Add)
            {
                return value1.GetValue() + value2.GetValue();
            }
            else if (method == EvalMethod.Subtract)
            {
                return value1.GetValue() - value2.GetValue();
            }
            else if (method == EvalMethod.Multiply)
            {
                return value1.GetValue() * value2.GetValue();
            }
            else if (method == EvalMethod.Divide)
            {
                return value1.GetValue() / value2.GetValue();
            }
            return default;
        }
    }
}
