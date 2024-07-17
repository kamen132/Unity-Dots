using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace EntitiesExample.CreatePrefab
{
    public struct CubePrefabGenData : IComponentData
    {
        public Entity Cube;
        public int GenCount;
    }
    public class PrefabGenAuthoring : MonoBehaviour
    {
        public GameObject CubePrefab;
        public int GenCount = 10;
        class Baker : Baker<PrefabGenAuthoring>
        {
            public override void Bake(PrefabGenAuthoring authoring)
            {
                var component=new CubePrefabGenData()
                {
                    Cube = GetEntity(authoring.CubePrefab,TransformUsageFlags.None),
                    GenCount = authoring.GenCount
                };
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), component);
            }
        }
    }
}
