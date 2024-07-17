using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace EntitiesExample.RotateCube
{
    public struct CubeRotateSpeed : IComponentData
    {
        public float Speed;
    }
    public class CubeRotateSpeedMono : MonoBehaviour
    {
        public int RotateSpeed = 360;
        public class Baker : Baker<CubeRotateSpeedMono>
        {
            public override void Bake(CubeRotateSpeedMono authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                CubeRotateSpeed speed = new CubeRotateSpeed()
                {
                    Speed = math.radians(authoring.RotateSpeed)
                };
                AddComponent(entity, speed);
            }
        }
    }
}
