using System.Collections.Generic;
using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public partial class ActionWrapper
    {
        protected TargetWrapper _GetTargets_Single(TargetWrapper target, TargetSelectorSingle selector)
        {
            if (selector.target == TargetType.CASTER)
                return TargetWrapper.Get(caster);
            else if (selector.target == TargetType.ATTACKER)
                return TargetWrapper.Get(target.attacker);
            else if (selector.target == TargetType.POINT)
                return TargetWrapper.Get(target.point);
            return target;
        }

        protected List<TargetWrapper> _GetTargets_Area(TargetWrapper target, TargetSelectorArea selector)
        {
            FVector3 center = FVector3.Zero;
            if (selector.center == AreaCenterType.CASTER)
                center = caster.position;
            else if (selector.center == AreaCenterType.TARGET)
                center = target.point;

            if (selector.targetType == TargetType.CASTER)
            {
                var list = new List<TargetWrapper>();
                list.Add(TargetWrapper.Get(caster));
                return list;
            }
            else if (selector.targetType == TargetType.POINT)
            {
                var list = new List<TargetWrapper>();
                for (int i = 0; i < selector.targetCount; i++)
                {
                    if (selector is TargetSelectorAreaCircle)
                    {
                        var circle = selector as TargetSelectorAreaCircle;
                        //FVector3 pos = Random.insideUnitCircle * circle.radius;
                        //pos += center;
                        //list.Add(TargetWrapper.Get(pos));
                    }
                    else if (selector is TargetSelectorAreaRing)
                    {
                        var ring = selector as TargetSelectorAreaRing;
                        //var dir = (Random.insideUnitCircle).normalized;
                        //FVector3 pos = (FVector3)dir * Random.Range(ring.innerRadius, ring.outerRadius) + center;
                        //list.Add(TargetWrapper.Get(pos));
                    }
                    else if (selector is TargetSelectorAreaSector)
                    {
                        //TODO: Random in sector
                    }
                    else if (selector is TargetSelectorAreaRect)
                    {
                        //TODO: Random in rect
                    }
                }

                return list;
            }
            else
            {
                var list = _GetTargets_Area_Entities(center, selector as TargetSelectorArea);

                if (selector.targetCount <= 0)
                    return list;
                if (list.Count > selector.targetCount)
                {
                    //TODO: SHUFFLE LIST
                    while (list.Count > selector.targetCount)
                    {
                        list.RemoveAt(0);
                    }
                }
                return list;
            }
        }

        protected bool _GetTargets_Area_Check(IUnit caster, IUnit target, FVector3 center,
            TargetSelectorAreaSector selector)
        {
            var _direction = caster.direction;
            if (center != caster.position)
            {
                //_direction = (center - caster.position).normalized;
            }
            //if (FVector3.Distance(center, target.position) < selector.radius
            //    && FVector3.Angle(_direction, (target.position - center).normalized) < selector.angle * 0.5f)
            //{
            //    return true;
            //}
            return false;
        }

        protected bool _GetTargets_Area_Check(IUnit caster, IUnit target, FVector3 center,
            TargetSelectorAreaCircle selector)
        {
            //if (FVector3.Distance(center, target.position) < selector.radius)
            //{
            //    return true;
            //}
            return false;
        }

        protected List<TargetWrapper> _GetTargets_Area_Entities(FVector3 center,
            TargetSelectorArea selector)
        {
            var units = _GetTargets_Area_OverlapAll(center, selector);
            if (units == null || units.Length <= 0)
                return _emptyWrappers;

            var lst = new List<TargetWrapper>();
            foreach (var unit in units)
            {
                if (IsTargetFlags(unit, selector.targetFlags) &&
                    IsTargetTeam(unit, selector.targetTeamType))
                {
                    lst.Add(TargetWrapper.Get(unit));
                }
            }
            return lst;
        }

        protected IUnit[] _GetTargets_Area_OverlapAll(FVector3 center, TargetSelectorArea area)
        {
            var dir = caster.direction;
           // if (center != caster.position)
                //dir = (center - caster.position).normalized;
            // dir = Quaternion.AngleAxis(area.rotAngle, FVector3.forward) * dir;
            var normalDir = FVector3.Cross(dir, FVector3.forward);
            var rotAngle = 1f; // FVector3.sin(FVector2.up, dir, FVector3.forward);
            var physics = runner.boot.physics;

            if (area is TargetSelectorAreaRect)
            {
                var rect = area as TargetSelectorAreaRect;
                var boxCenter = center - normalDir * rect.width * rect.pivot.x - dir * rect.length * rect.pivot.y;
                // DrawRect(boxCenter, new FVector2(rect.width, rect.length), rotAngle);
                //var units = physics.OverlapBoxAll(boxCenter,
                //    new FVector2(rect.width, rect.length), rotAngle);
                // return units;
            }
            else if (area is TargetSelectorAreaCircle)
            {
                var circle = area as TargetSelectorAreaCircle;
                var circleCenter = center - normalDir * circle.radius * circle.pivot.x - dir * circle.radius * circle.pivot.y;
                //DrawCircle(circleCenter, circle.radius);
                //var units = physics.OverlapCircleAll(circleCenter, circle.radius);
                //return units;
            }
            else if (area is TargetSelectorAreaRing)
            {
                //TODO: TargetSelectorAreaRing
                return null;
            }
            else if (area is TargetSelectorAreaSector)
            {
                //TODO: TargetSelectorAreaSector
                return null;
            }
            else if (area is TargetSelectorAreaSectorArray)
            {
                //TODO: TargetSelectorAreaSectorArray
                return null;
            }
            return null;
        }

        private bool IsTargetTeam(IUnit unit, TargetTeamType targetTeamType)
        {
            var teamType = owner.GetTargetTeamType(unit);
            return targetTeamType.HasFlag(teamType);
        }

        private bool IsTargetFlags(IUnit unit, TargetFlags targetFlags)
        {
            return targetFlags.HasFlag(unit.flags);
        }

        private List<TargetWrapper> _GetTargets_Points(TargetWrapper target, TargetSelectorPoints selector)
        {
            //TODO: TargetSelectorCustomPoints
            return _emptyWrappers;
        }

        private static List<TargetWrapper> _emptyWrappers = new List<TargetWrapper>();
        protected List<TargetWrapper> GetTargets(TargetWrapper target, TargetSelector selector)
        {
            if (selector == null)
            {
                return new List<TargetWrapper>() { TargetWrapper.Get(caster) };
            }
            else if (selector is TargetSelectorSingle)
            {
                var list = new List<TargetWrapper>();
                list.Add(_GetTargets_Single(target, selector as TargetSelectorSingle));
                return list;
            }
            else if (selector is TargetSelectorArea)
            {
                return _GetTargets_Area(target, selector as TargetSelectorArea);
            }
            else if (selector is TargetSelectorPoints)
            {
                return _GetTargets_Points(target, selector as TargetSelectorPoints);
            }
            return _emptyWrappers;
        }
    }
}
