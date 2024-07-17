using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace EntitiesExample.CreatePrefab
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CreatePrefabSystemGroup))]
    public partial class PrefabGenSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var genData in SystemAPI.Query<RefRO<CubePrefabGenData>>())
            {
                var cubes = CollectionHelper.CreateNativeArray<Entity>(genData.ValueRO.GenCount * genData.ValueRO.GenCount, Allocator.Temp);
                EntityManager.Instantiate(genData.ValueRO.Cube, cubes);
                int index = 0;
                int length = genData.ValueRO.GenCount;
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        var trans = SystemAPI.GetComponentRW<LocalTransform>(cubes[index]);
                        index++;
                        trans.ValueRW.Position = new float3(i + 2, 0, j + 2);
                    }
                }
                cubes.Dispose();
                Enabled = false;
            }
        }
    }
}
