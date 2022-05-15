namespace SkillSystem
{
    public class TargetSelector
    {
        public AbilityAction owner { get; private set; }
        public Ability ability => owner.ability;

        public virtual void Reset(AbilityAction action)
        {
            this.owner = action;

            ability.ApplyValue(this);
        }
    }
}
