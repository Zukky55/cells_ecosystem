using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace CellsEcosystem.SandBox.ECS
{
    public class LevelUpSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref LevelComponent levelComponent) =>
        {
            levelComponent.level += 1f * UnityEngine.Time.deltaTime;
            //Debug.Log(levelComponent.level);
        }).Run();
    }
}
}