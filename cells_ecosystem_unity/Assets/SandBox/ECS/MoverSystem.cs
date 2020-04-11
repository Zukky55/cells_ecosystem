using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
namespace CellsEcosystem.SandBox.ECS
{
    public class MoverSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent) =>
            {
                translation.Value.y += moveSpeedComponent.moveSpeed * UnityEngine.Time.deltaTime;
                if (translation.Value.y > 5f)
                {
                    moveSpeedComponent.moveSpeed = -math.abs(moveSpeedComponent.moveSpeed);
                }

                if (translation.Value.y < -5f)
                {
                    moveSpeedComponent.moveSpeed = +math.abs(moveSpeedComponent.moveSpeed);
                }
            }).Run();
        }
    }
}