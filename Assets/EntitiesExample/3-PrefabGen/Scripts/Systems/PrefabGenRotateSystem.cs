using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace EntitiesExample.CreatePrefab
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CreatePrefabSystemGroup))]
    public partial class PrefabGenRotateSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var trans in SystemAPI.Query<RotateAndMoveAspect>())
            {
                trans.RotationAndMove(math.radians(60.0f), deltaTime);
            }
        }
    }
}
