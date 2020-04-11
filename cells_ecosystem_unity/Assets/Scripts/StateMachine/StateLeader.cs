using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The ToDie state behaivor.
    /// </summary>
    public class StateLeader : State<Cell, CellState>
    {
        public StateLeader(Cell owner, CellState identity) : base(owner, identity) { }

        Transform target;
        public override void Enter()
        {
            base.Enter();
        }
        public override void Execute()
        {
            if (owner.TryNeablyEnemyDetection())
            {
                return;
            }

            if (target == null)
            {
                target = owner.TryNeablyAtomDetection()?.transform;
            }
            if (target)
            {
                var targetVec = target.position - owner.transform.position;
                owner.Move(targetVec.normalized);
            }
        }
        public override void Exit()
        {

        }
    }
}