using Unity.Entities;
using UnityEngine;
namespace EntitiesExample.TrasnsformAspect
{
    public struct TagCubeA : IComponentData
    {

    }

    public class TagCubeAuthoring : MonoBehaviour
    {
        public class Baker : Baker<TagCubeAuthoring>
        {
            public override void Bake(TagCubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var tagB = new TagCubeA();
                AddComponent(entity, tagB);
            }
        }
    }
}
