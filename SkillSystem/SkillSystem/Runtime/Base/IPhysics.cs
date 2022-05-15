using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public interface IPhysics
    {
        IUnit[] OverlapBoxAll(FVector2 point, FVector2 size, FP angle, int layerMask = -1);
        IUnit[] OverlapCircleAll(FVector2 point, FP radius, int layerMask = -1);
    }
}
