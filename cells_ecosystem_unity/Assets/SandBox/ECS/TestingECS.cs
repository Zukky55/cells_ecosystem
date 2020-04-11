using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;
namespace CellsEcosystem.SandBox.ECS
{
    public class TestingECS : MonoBehaviour
    {
        [SerializeField]
        Mesh mesh;
        [SerializeField]
        Material material;

        private void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(LevelComponent),
                typeof(Translation),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(RenderMesh),
                typeof(MoveSpeedComponent)
                );

            NativeArray<Entity> entityArray = new NativeArray<Entity>(100000, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, entityArray);
            for (int i = 0; i < entityArray.Length; i++)
            {
                Entity entity = entityArray[i];

                entityManager.SetComponentData(entity,
                    new LevelComponent
                    {
                        level = UnityEngine.Random.Range(10, 20)
                    });

                entityManager.SetComponentData(entity,
                    new MoveSpeedComponent
                    {
                        moveSpeed = UnityEngine.Random.Range(1f, 2f)
                    });

                entityManager.SetComponentData(entity,
                    new Translation
                    {
                        Value = new float3(UnityEngine.Random.Range(-8, 8f), UnityEngine.Random.Range(-5, 5f), 0f)
                    });


                entityManager.SetSharedComponentData(entity, new RenderMesh
                {
                    mesh = this.mesh,
                    material = this.material,
                });
            }

            entityArray.Dispose();
        }
    }
}