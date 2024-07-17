using Unity.Entities;
using UnityEngine;
namespace EntitiesExample.TrasnsformAspect
{
    public struct TagCubeB : IComponentData
    {

    }

    public class TagCubeBAuthoring : MonoBehaviour
    {
        public class Baker : Baker<TagCubeBAuthoring>
        {
            public override void Bake(TagCubeBAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var tagB = new TagCubeB();
                AddComponent(entity, tagB);
            }
        }
    }
}
