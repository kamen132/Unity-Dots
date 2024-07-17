using Unity.Entities;
using NotImplementedException = System.NotImplementedException;



namespace Entities.System
{
    //-----非托管系统------
    public partial struct ExampleSystem1 : ISystem , ISystemStartStop
    {
        //系统事件回调，用于在使用前初始化系统及其数据。
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        //系统事件回调添加您的系统必须每帧执行的工作。
        public void OnUpdate(ref SystemState state)
        {
           
        }
        
        //系统事件回调，用于在销毁之前清理资源。
        public void OnDestroy(ref SystemState state)
        {
           
        }
        
        //--选择性地实现 ISystemStartStop
        //	第一次调用之前OnUpdate以及系统停止或禁用后恢复时的系统事件回调。
        public void OnStartRunning(ref SystemState state)
        {
          
        }
        //当系统被禁用或者与系统更新所需的任何组件不匹配时，系统事件回调。
        public void OnStopRunning(ref SystemState state)
        {
           
        }
    }

    //------托管系统------
    public partial class EntitiesSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
        }
        protected override void OnUpdate()
        {
            
        }
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
