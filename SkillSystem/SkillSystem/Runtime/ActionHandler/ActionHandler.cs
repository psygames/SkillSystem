using System.Collections.Generic;
using System.Reflection;

namespace SkillSystem.Runtime
{
    public class ActionHandler
    {
        private Dictionary<string, MethodInfo> handler_methods = new Dictionary<string, MethodInfo>();
        private object[] handler_method_args = new object[2];

        public void Handle(ActionWrapper wrapper, AbilityAction action)
        {
            if (action == null)
            {
                Log.Error("Handle Action is null ");
                return;
            }

            var actionType = action.GetType();
            var functionName = actionType.Name;

            if (!handler_methods.TryGetValue(functionName, out var method))
            {
                method = GetType().GetMethod(functionName);
                handler_methods.Add(functionName, method);
            }

            if (method == null)
            {
                Log.Error($"Not Exist Action Handle : {functionName}");
                return;
            }

            handler_method_args[0] = wrapper;
            handler_method_args[1] = action;
            method.Invoke(this, handler_method_args);
        }
    }
}
