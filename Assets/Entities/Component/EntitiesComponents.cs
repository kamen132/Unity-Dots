using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
namespace Entities.Component
{
    #region 1.UnmanagedComponents --- 非托管组件存储最常见的数据类型

    /*1.UnmanagedComponents
    非托管类型*/
    public struct UnmanagedComponentsData : IComponentData
    {
        public int Value;
    }

    #endregion

    #region 2.ManagedComponents --- 与非托管组件不同，托管组件可以存储任何类型的属性。

    /*2.ManagedComponents
    托管类型 
    最好继承IDisposable,ICloneable接口
    托管组件的限制：
    - 不能在jobs中访问
    - 不能使用Burst编译
    - 需要垃圾回收（GC）
    - 为了序列化，必需包含一个无参数的构造函数*/
    public class ManagedComponentsData : IComponentData, IDisposable, ICloneable
    {
        public int Value;
        public void Dispose()
        {

        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region 3.SharedComponents --- 共享组件根据其共享组件的值将实体分组，这有助于数据重复删除。

    //struct继承ISharedComponentData， 创建非托管组件
    public struct SharedComponentsData : ISharedComponentData
    {
        public int Value;
    }

    //struct继承ISharedComponentData和IEquatable<>，实现GetHashCode， 创建托管组件：
    public struct SharedComponentsData2 : ISharedComponentData, IEquatable<SharedComponentsData2>
    {
        public int Value;
        public bool Equals(SharedComponentsData2 other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    #endregion

    #region 4.CleanupComponents

    /*struct继承ICleanupComponentData：
    当你销毁一个包含一个Cleanup component的实体时，
    Unity会删除所有Cleanup component。这个实体仍然存在，直到你从它里面移除所有的Cleanup component。
    这对于标记那些在销毁时需要清理的实体很有用。*/
    public struct CleanupComponentsData : ICleanupComponentData
    {
        public int Value;
    }

    public partial class CleanupComponentsDataExample : SystemBase
    {
        public void CleanupComponentMono()
        {
            /*// 创建一个包含清理组件的实体。
            Entity e = EntityManager.CreateEntity(
                typeof(Translation), typeof(Rotation), typeof(ExampleCleanup));

            // 尝试销毁该实体，但由于该实体具有清理组件，Unity实际上不会销毁该实体。相反，Unity只是移除了Translation和Rotation组件。
            EntityManager.DestroyEntity(e);

            // 实体仍然存在，这表明您仍然可以正常使用该实体。
            EntityManager.AddComponent<Translation>(e);

            // 从实体中移除所有组件。这将销毁实体。
            EntityManager.DestroyEntity(e, new ComponentTypes(typeof(ExampleCleanup), typeof(Translation)));

            // 证明实体不再存在。entityExists 为 false。
            bool entityExists = EntityManager.Exists(e);*/
        }
        protected override void OnUpdate()
        {

        }
    }

  #endregion

    #region 5.CleanupSharedComponents

    /*Cleanup shared components 是托管的 shared components，
     struct继承ICleanupSharedComponentData：
     当清理实时时需要相同信息时用此结构。*/
    public class CleanupSharedComponentsData : ICleanupComponentData
    {

    }

  #endregion

    #region 6.TagComponents ??? 功能待定 无任何定义

    /*Tag components是非托管组件，无数据，不占用空间*/
    public class TagComponentsData : IComponentData
    {

    }

  #endregion

    #region 7.DynamicBufferComponents 非托管变长数组结构，用于entity存储数组数据

    /*dynamic buffer component是非托管变长数组结构，
     用于entity存储数组数据，例如entity的寻路位置点。*/

    public struct DynamicBufferComponentsData : IBufferElementData
    {
        public int Value;
    }

    public struct SomeComponentsData : IComponentData
    {

    }

    public class DynamicBufferComponentsDataExample1
    {
        public void GetDynamicBufferComponentsData(Entity e)
        {
            EntityManager world = new EntityManager();
            DynamicBuffer<DynamicBufferComponentsData> myBuffer = world.GetBuffer<DynamicBufferComponentsData>(e);
        }
    }

    /*使用ArchetypeChunk.GetBufferAccessor方法，
    参数为：BufferTypeHandle<T> ，返回值为BufferAccessor<T>， 
    遍历BufferAccessor<T>可得到具体的dynamic buffers：*/
    public partial class DynamicBufferComponentsDataSystem : SystemBase
    {
        public void BaseMethod(Entity e)
        {
            //创建一个动态缓冲区的实体。
            EntityManager.CreateEntity(typeof(DynamicBufferComponentsData));

            //给实体e添加一个动态数组。
            EntityManager.AddComponent<DynamicBufferComponentsData>(e);

            //移除实体e的动态数组DynamicBufferComponentsData。
            EntityManager.RemoveComponent<DynamicBufferComponentsData>(e);

            //匹配实体的动态数组DynamicBufferComponentsData的查询。
            EntityQuery query = GetEntityQuery(typeof(DynamicBufferComponentsData));

            //获取实体e的动态数组
            DynamicBuffer<DynamicBufferComponentsData> myBuff = EntityManager.GetBuffer<DynamicBufferComponentsData>(e);

            //读取和写入长度5的数据，如果超出抛出异常。
            int x = myBuff[5].Value;
            myBuff[5] = new DynamicBufferComponentsData {Value = x + 1};

            //在当前Length处增加一个新的数值，并增加Length，如果超过了Capacity大小，则缓冲区大小调整为Capacity的两倍。
            myBuff.Add(new DynamicBufferComponentsData {Value = 100});

            //设置了可用索引为10（0-9）
            myBuff.Length = 10;

            //设置数组大小，如果小于Length则抛出安全检查异常。
            myBuff.Capacity = 20;

            //在ForEach中使用
            Entities.ForEach((in DynamicBuffer<DynamicBufferComponentsData> myBuff) =>
            {
                for (int i = 0; i < myBuff.Length; i++)
                {
                    // ... read myBuff[i]
                }
            }).Schedule();

            //如果要修改就改成ref。
            Entities.ForEach((ref DynamicBuffer<DynamicBufferComponentsData> myBuff) =>
            {
                for (int i = 0; i < myBuff.Length; i++)
                {
                    myBuff.Add(new DynamicBufferComponentsData() {Value = 1});
                }
            }).Schedule();


            //--------通过 BufferFromEntity 随机查找缓冲区-----------//

            //如果一个实体的所有实体都Entities.ForEach需要相同的缓冲区，则可以将该缓冲区捕获为主线程上的局部变量：
            DynamicBuffer<DynamicBufferComponentsData> eBuffer = EntityManager.GetBuffer<DynamicBufferComponentsData>(e);
            Entities.ForEach((in SomeComponentsData someComp) =>
            {
                // ... use eBuffer
                for (int i = 0; i < eBuffer.Length; i++)
                {
                    //...
                }
            }).Schedule();

            /*
             并行记录
            如果使用ScheduleParallel，请注意不能并行写入缓冲区。
            但是，您可以使用 EntityCommandBuffer.ParallelWriter来并行记录更改。
            */


            /*
             OnUpdate中在ForEach中访问多个缓冲区
            如果Entities.ForEach需要在其代码中查找一个或多个缓冲区，则需要一个BufferFromEntity结构，它提供按实体对缓冲区的随机查找。
            在SystemBase类中的OnUpdate中
            */
            BufferLookup<DynamicBufferComponentsData> lookup = GetBufferLookup<DynamicBufferComponentsData>();
            Entities.ForEach((in SomeComponentsData someComp) =>
            {
                // EntityManager不能再作业中使用，我所我们使用, 所以我们可以用这种方式
                DynamicBuffer<DynamicBufferComponentsData> myBuff = lookup[e];
                // ... use myBuff
            }).Schedule();


            /*-----------------使用动态缓冲区修改---------------------*/
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

            //移除e的DynamicBufferComponentsData
            ecb.RemoveComponent<DynamicBufferComponentsData>(e);

            //记录向现有实体添加DynamicBufferComponentsData动态缓冲区的命令。
            //返回的DynamicBuffer的数据存储在EntityCommandBuffer中，
            //因此对返回缓冲区的更改也会被记录下来。
            DynamicBuffer<DynamicBufferComponentsData> myBuff2 = ecb.AddBuffer<DynamicBufferComponentsData>(e);

            //实体将有一个DynamicBufferComponentsData缓冲区
            //长度20和这些记录的值。
            myBuff2.Length = 20;
            myBuff2[0] = new DynamicBufferComponentsData {Value = 5};
            myBuff2[3] = new DynamicBufferComponentsData {Value = -9};

            // SetBuffer类似于AddBuffer，但是安全检查将在回放时抛出异常，因为实体还没有DynamicBufferComponentsData缓冲区。Add过才能Set。
            DynamicBuffer<DynamicBufferComponentsData> otherBuf = ecb.SetBuffer<DynamicBufferComponentsData>(e);

            //记录一个要追加到缓冲区的DynamicBufferComponentsData值。抛出异常
            //实体还没有DynamicBufferComponentsData缓冲区。
            ecb.AppendToBuffer<DynamicBufferComponentsData>(e, new DynamicBufferComponentsData {Value = 12});
        }
        protected override void OnUpdate()
        {
            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAllRW<DynamicBufferComponentsData>()
                .Build(EntityManager);
            NativeArray<ArchetypeChunk> chunks = query.ToArchetypeChunkArray(Allocator.Temp);
            for (int i = 0; i < chunks.Length; i++)
            {
                UpdateChunk(chunks[i]);
            }
            chunks.Dispose();
        }

        private void UpdateChunk(ArchetypeChunk chunk)
        {
            /*-----------------获取块（Chunk）的所有缓冲区---------------------*/
            // ... 假设一个带有MyElement动态缓冲区的块

            // 从SystemBase获取一个表示动态缓冲区类型MyElement的BufferTypeHandle
            BufferTypeHandle<DynamicBufferComponentsData> myElementHandle = GetBufferTypeHandle<DynamicBufferComponentsData>();

            // 从块中获取 BufferAccessor .
            BufferAccessor<DynamicBufferComponentsData> buffers = chunk.GetBufferAccessor(myElementHandle);

            //遍历区块中每个实体的所有MyElement缓冲区。
            for (int i = 0; i < chunk.Count; i++)
            {
                DynamicBuffer<DynamicBufferComponentsData> buffer = buffers[i];

                //遍历缓冲区中的所有元素。
                for (int j = 0; j < buffer.Length; j++)
                {
                    // ...
                }
            }
        }
    }

  #endregion

    #region 8.ChunkComponents --- 一种存储每个块而不是每个实体的值的组件

    //Chunk组件是一种存储每个块而不是每个实体的值的组件。它们提供与共享组件类似的功能，但在一些基本方面有所不同。
    public struct ExampleComponent : IComponentData
    {

    }

    public struct ExampleChunkComp : IComponentData
    {
        public int Value;
    }

    /*
    与其他组件类型相比，Chunk 组件使用一组不同的 API 来添加、移除、获取和设置它们。
    例如，要将 Chunk 组件添加到实体，您可以使用EntityManager.AddChunkComponentData而不是常规的EntityManager.AddComponent。

    以下代码示例展示了如何添加、设置和获取块组件。
    它假设存在一个名为的块组件ExampleChunkComp和一个名为的非块组件ExampleComponent：*/
    public partial class ChunkComponentsSystem : SystemBase
    {
        private void ChunkComponentExample(Entity e)
        {
            // 将 ExampleChunkComp 添加到传入实体的 chunk 中。
            EntityManager.AddChunkComponentData<ExampleChunkComp>(e);

            // 查找所有具有 ExampleComponent 和 ExampleChunkComponent 的 chunk。
            // 为了区分 chunk 组件和普通的 IComponentData，必须使用 ComponentType.ChunkComponent 来指定 chunk 组件。
            EntityQuery query = GetEntityQuery(typeof(ExampleComponent), ComponentType.ChunkComponent<ExampleChunkComp>());
            NativeArray<ArchetypeChunk> chunks = query.ToArchetypeChunkArray(Allocator.Temp);

            // 设置第一个 chunk 的 ExampleChunkComp 值。
            EntityManager.SetChunkComponentData<ExampleChunkComp>(chunks[0], new ExampleChunkComp {Value = 6});

            // 获取第一个 chunk 的 ExampleChunkComp 值。
            ExampleChunkComp exampleChunkComp = EntityManager.GetChunkComponentData<ExampleChunkComp>(chunks[0]);
            Debug.Log(exampleChunkComp.Value); // 6
        }

        //你也可以通过任意一个块的实体来获取和设置该块的块组件：

        struct MyChunkComp : IComponentData
        {
            public int Value;
        }

        private void ChunkComponentExample2(ArchetypeChunk e)
        {
            // Sets the ExampleChunkComp value of the entity's chunk.
            var chuck = new MyChunkComp {Value = 6};
            EntityManager.SetChunkComponentData<MyChunkComp>(e, chuck);

            // Sets the ExampleChunkComp value of the entity's chunk.
            MyChunkComp myChunkComp = EntityManager.GetChunkComponentData<MyChunkComp>(e);
            Debug.Log(myChunkComp.Value); // 6
        }

        protected override void OnUpdate()
        {

        }
    }

  #endregion

    #region EnableableComponents

    public struct Health : IComponentData
    {
    }

    public partial struct EnableableComponentSystem : ISystem
    {

        public void OnUpdate(ref SystemState system)
        {

            /*Entity e = system.EntityManager.CreateEntity(typeof(Health));

            ComponentLookup<Health> healthLookup = system.GetComponentLookup<Health>();

            // true
            bool b = healthLookup.IsComponentEnabled(e);

            // disable the Health component of the entity
            healthLookup.SetComponentEnabled(e, false);

            // though disabled, the component can still be read and modified
            Health h = healthLookup[e];*/

        }

    }

  #endregion
}
