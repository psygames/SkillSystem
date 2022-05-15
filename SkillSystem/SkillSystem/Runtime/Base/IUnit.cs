using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public interface IUnit
    {
        FVector3 position { get; }
        FVector3 direction { get; }

        AbilityManager abilityManager { get; }
        ModifierManager modifierManager { get; }

        void Update(FP deltaTime);

        void ApplyModifierState(ModifierState state);
        void UnapplyModifierState(ModifierState state);

        void ApplyProperty(ModifierProperty property);
        void UnapplyProperty(ModifierProperty property);

        TargetFlags flags { get; }
        TargetTeamType GetTargetTeamType(IUnit target);
    }
}
