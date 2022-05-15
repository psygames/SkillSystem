using System;
using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public class DelayedActionRunner
    {
        public bool isExpired => cooldown <= 0;
        public FP cooldown { get; private set; }
        public Action callback { get; private set; }

        public void Update(FP deltaTime)
        {
            if (isExpired)
                return;
            cooldown = FMath.Max(0, cooldown - deltaTime);
            if (isExpired)
            {
                callback?.Invoke();
            }
        }

        public void Release()
        {
            this.cooldown = 0;
            this.callback = null;
            // pool.Release(this);
        }

        static ObjectPool<DelayedActionRunner> pool = null;
        public static DelayedActionRunner Get(FP delay, Action callback)
        {
            //var runner = pool.Get();
            //runner.cooldown = delay;
            //runner.callback = callback;
            //return runner;
            return null;
        }
    }
}
