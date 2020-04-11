using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace CellsEcosystem.SandBox.DOTS_Changed
{
    public class PlayerMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            JobHandle jobHandle = Entities.ForEach((ref Translation translation, in MoveDirection moveDirection) =>
            {
                translation.Value.x += moveDirection.Value * deltaTime;
            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}