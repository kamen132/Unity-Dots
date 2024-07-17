using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace EntitiesExample.CreatePrefab
{
    readonly partial struct RotateAndMoveAspect : IAspect
    {
        readonly RefRW<LocalTransform> mTrans;

        public void RotationAndMove(float speed,float deltaTime)
        {
            mTrans.ValueRW.Position.y = math.sin(deltaTime * speed);
            mTrans.ValueRW = mTrans.ValueRO.RotateY(speed * deltaTime);
        }
    }
}
