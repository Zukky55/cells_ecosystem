using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Player state behaivor.
    /// </summary>
    public class StatePlayer : State<Cell, CellState>
    {
        public StatePlayer(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Execute()
        {
            if (!owner.IsPlayer)
            {
                owner.ChangeState(CellState.Leader);
            }
        }
        public override void Exit()
        {
        }
    }
}