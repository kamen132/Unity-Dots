using EntitiesExample.TrasnsformAspect;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformAspectSystemGroup))]
public partial class TransformAspectExample : SystemBase
{
    EntityQuery queryA ;
    EntityQuery queryB ;
    protected override void OnCreate()
    {
        base.OnCreate();
        queryA = new EntityQueryBuilder(Allocator.Temp).WithAny<TagCubeA>().Build(this);
        queryB = new EntityQueryBuilder(Allocator.Temp).WithAny<TagCubeB>().Build(this);
    }
    protected override void OnUpdate()
    {
        var arrtEntityA = queryA.ToEntityArray(Allocator.Persistent);
        var arrtEntityB = queryB.ToEntityArray(Allocator.Persistent);
        var transA = EntityManager.GetComponentData<LocalTransform>(arrtEntityA[0]);
        var transB = EntityManager.GetComponentData<LocalTransform>(arrtEntityB[0]);
        var distance = Vector3.Distance(transA.Position, transB.Position);
        Debug.LogError($"distance :" + distance);
    }
}
