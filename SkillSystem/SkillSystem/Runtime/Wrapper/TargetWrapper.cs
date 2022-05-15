using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public class TargetWrapper
    {
        public FVector3 point => isPoint ? _point : target.position;
        public IUnit caster { get; private set; }
        public IUnit target { get; private set; }
        public IUnit attacker { get; private set; }

        private FVector3 _point;

        public bool isUnit => caster != null || target != null || attacker != null;
        public bool isPoint => !isUnit;

        public static TargetWrapper Get(IUnit unit)
        {
            return Get(null, unit, null, unit.position);
        }

        public static TargetWrapper Get(FVector3 point)
        {
            return Get(null, null, null, point);
        }

        public static TargetWrapper Get(IUnit caster, IUnit target, IUnit attacker, FVector3 point)
        {
            var _t = new TargetWrapper();
            _t.caster = caster;
            _t.target = target;
            _t.attacker = attacker;
            _t._point = point;
            return _t;
        }
    }
}
