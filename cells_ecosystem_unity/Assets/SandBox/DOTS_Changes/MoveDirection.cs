using Unity.Entities;

namespace CellsEcosystem.SandBox.DOTS_Changed
{
    [GenerateAuthoringComponent]
    public struct MoveDirection : IComponentData
    {
        public float Value;
    }
}