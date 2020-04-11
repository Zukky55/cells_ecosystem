using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Escape state behaivor.
    /// </summary>
    public class StateEscape : State<Cell, CellState>
    {
        public StateEscape(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Execute()
        {
            if (!owner.IsStatusComplete()) return;

        }
        public override void Exit()
        {
        }
    }
}