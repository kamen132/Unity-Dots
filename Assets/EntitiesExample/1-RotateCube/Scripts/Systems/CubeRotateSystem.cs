using EntitiesExample.CreatePrefab;
using Unity.Entities;
using Unity.Transforms;
namespace EntitiesExample.RotateCube
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CubeRotateSystemGroup))]
    public partial class CubeRotateSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (transform,speed) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<CubeRotateSpeed>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.Speed * deltaTime);
            }
        }
    }
}
