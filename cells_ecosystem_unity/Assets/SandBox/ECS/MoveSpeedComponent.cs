using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace CellsEcosystem.SandBox.ECS
{
    public struct MoveSpeedComponent : IComponentData
    {
        public float moveSpeed;
    }
}