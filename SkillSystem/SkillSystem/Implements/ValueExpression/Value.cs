using SkillSystem.Common;

namespace SkillSystem
{
    public abstract class Value
    {
        public Ability owner { get; private set; }
        public abstract FP GetValue();

        public virtual void Reset(Ability owner)
        {
            this.owner = owner;
        }

        public static implicit operator FP(Value val)
        {
            if (val == null)
                return 0;
            return val.GetValue();
        }
    }
}
