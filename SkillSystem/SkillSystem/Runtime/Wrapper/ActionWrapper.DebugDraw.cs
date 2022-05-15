using SkillSystem.Common;

namespace SkillSystem.Runtime
{
    public partial class ActionWrapper
    {
        private void DrawRect(FVector2 center, FVector2 size, FP angle)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //TileMap3D.WorldUtil.Vector2To3(center.x, center.y, out var scenePos);
            //cube.transform.position = scenePos;
            //cube.transform.localScale = new Vector3(size.x, 0.1f, size.y);
            //cube.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
            //DOVirtual.DelayedCall(1, () =>
            //{
            //    GameObject.Destroy(cube);
            //});
        }

        private void DrawCircle(FVector2 center, FP radius)
        {
            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            //TileMap3D.WorldUtil.Vector2To3(center.x, center.y, out var scenePos);
            //cube.transform.position = scenePos;
            //cube.transform.localScale = new Vector3(radius * 2, 0.1f, radius * 2);
            //DOVirtual.DelayedCall(1, () =>
            //{
            //    GameObject.Destroy(cube);
            //});
        }

        private void DrawRing(FVector2 center, FP innerRadius, FP outerRadius)
        {
            var steps = 360;
            for (int i = 0; i < steps + 1; i++)
            {
                var a = i;
                //FVector2 dir = Quaternion.AngleAxis(a, Vector3.forward) * Vector3.up;
                //var c = center + dir * (outerRadius + innerRadius) * 0.5f;
                //DrawRect(c, new Vector2(0.05f, (outerRadius - innerRadius)), a);
            }
        }

        private void DrawSector(FVector2 center, FP radius, FP angle, FP rotAngle)
        {
            var steps = (int)angle;
            for (int i = 0; i < steps + 1; i++)
            {
                var a = rotAngle + angle * (1f * i / steps - 0.5f);
                //FVector2 dir = Quaternion.AngleAxis(a, Vector3.forward) * Vector3.up;
                //var c = center + dir * radius * 0.5f;
                //DrawRect(c, new Vector2(0.05f, radius), a);
            }
        }
    }
}
