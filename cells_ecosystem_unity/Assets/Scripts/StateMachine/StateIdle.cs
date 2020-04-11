using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Idle state behaivor.
    /// </summary>
    public class StateIdle : State<Cell, CellState>
    {
        public StateIdle(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
            owner.Rb.velocity = Vector3.zero;
        }
        public override void Execute()
        {
            if (!owner.IsStatusComplete()) return;
            if (ElapsedTimeSinseStateStart > owner.Tribe.Idle.IdleTime)
            {
                owner.ChangeState(CellState.Wander);
            }
        }
        public override void Exit() { }
    }
}
