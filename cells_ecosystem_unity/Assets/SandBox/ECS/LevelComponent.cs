using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace CellsEcosystem.SandBox.ECS
{
    public struct LevelComponent : IComponentData
    {
        public float level;
    }
}