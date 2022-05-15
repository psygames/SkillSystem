using System;
using System.Collections.Generic;
using System.Text;
using SkillSystem.Common;

namespace SkillSystem
{
    public class ExpressionValue : NamedValue
    {
        public string value;

        public Value builtValue { get; private set; }
        public List<NamedValue> tempValues { get; private set; }

        public ExpressionValue() { }
        public ExpressionValue(string name, string value) : base(name)
        {
            this.value = value;
        }

        public override FP GetValue()
        {
            if (builtValue == null)
            {
                builtValue = Build(value, owner.values, owner.referenceValues, tempValues);
            }
            return builtValue.GetValue();
        }

        public override void Reset(Ability owner)
        {
            base.Reset(owner);
            this.builtValue = null;
        }

        public void SetTempValues(List<NamedValue> values)
        {
            this.tempValues = values;
        }

        public static Value Build(string str, List<NamedValue> customValues, List<ReferenceValue> referenceValues, List<NamedValue> tempValues)
        {
            var simpleStrBulder = new StringBuilder();
            List<Value> innerValues = null;
            var _intent = 0;
            var _st = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    _intent++;

                    if (_intent == 1)
                    {
                        _st = i;
                    }
                }
                else if (str[i] == ')')
                {
                    _intent--;
                    if (_intent == 0)
                    {
                        if (innerValues == null)
                            innerValues = new List<Value>();
                        var v = Build(str.Substring(_st + 1, i - _st - 1), customValues, referenceValues, tempValues);
                        innerValues.Add(v);
                        simpleStrBulder.Append($"#{innerValues.Count - 1}");
                    }
                }
                else if (_intent == 0)
                {
                    simpleStrBulder.Append(str[i]);
                }
            }

            var simpleStr = simpleStrBulder.ToString();
            List<Value> vals = null;
            List<EvalMethod> meths = null;

            _st = 0;

            for (int i = 0; i < simpleStr.Length; i++)
            {
                var c = simpleStr[i];
                if (c == '*' || c == '/' || c == '+' || c == '-')
                {
                    if (vals == null) vals = new List<Value>();
                    if (meths == null) meths = new List<EvalMethod>();

                    var str_val = simpleStr.Substring(_st, i - _st);
                    if (c == '-' && str_val.Trim() == "")
                    {
                    }
                    else
                    {
                        vals.Add(ParseValue(str_val, innerValues, customValues, referenceValues, tempValues));
                        meths.Add(ParseEvalMethod(c));
                        _st = i + 1;
                    }
                }
            }

            var latest_val_str = simpleStr.Substring(_st, simpleStr.Length - _st);
            var latest_val = ParseValue(latest_val_str, innerValues, customValues, referenceValues, tempValues);

            if (meths == null)
                return latest_val;

            vals.Add(latest_val);

            var count = meths.Count;
            for (int i = 0; i < count; i++)
            {
                var index = i - count + meths.Count;
                var m = meths[index];
                if (m == EvalMethod.Multiply || m == EvalMethod.Divide)
                {
                    vals[index + 1] = new EvalValue(vals[index], vals[index + 1], m);
                    vals.RemoveAt(index);
                    meths.RemoveAt(index);
                }
            }

            count = meths.Count;
            for (int i = 0; i < count; i++)
            {
                var index = i - count + meths.Count;
                var m = meths[index];
                if (m == EvalMethod.Add || m == EvalMethod.Subtract)
                {
                    vals[index + 1] = new EvalValue(vals[index], vals[index + 1], m);
                    vals.RemoveAt(index);
                    meths.RemoveAt(index);
                }
            }
            return vals[0];
        }

        private static EvalMethod ParseEvalMethod(char c)
        {
            if (c == '+')
                return EvalMethod.Add;
            if (c == '-')
                return EvalMethod.Subtract;
            if (c == '*')
                return EvalMethod.Multiply;
            if (c == '/')
                return EvalMethod.Divide;
            return default;
        }

        private static Value ParseValue(string str, List<Value> innerValues, List<NamedValue> customValues, List<ReferenceValue> referenceValues, List<NamedValue> tempValues)
        {
            str = str.Trim();
            if (str[0] >= '0' && str[0] <= '9'
                || str[0] == '-' && str[1] >= '0' && str[1] <= '9')
            {
                if (float.TryParse(str, out var val))
                    return new FloatValue(val.ToString(), val);
                throw new ArgumentException("数值转换错误: " + str);
            }

            if (str[0] == '#')
            {
                return innerValues[int.Parse(str.Substring(1))];
            }

            var refVal = tempValues?.Find(a => a.name == str);
            if (refVal == null)
                refVal = customValues?.Find(a => a.name == str);
            if (refVal == null)
                refVal = referenceValues?.Find(a => a.name == str);
            if (refVal == null)
                throw new ArgumentException("引用数据不存在: " + str);
            return refVal;
        }
    }
}
