using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Attack state behaivor.
    /// </summary>
    public class StateAttack : State<Cell, CellState>
    {
        public StateAttack(Cell owner, CellState identity) : base(owner, identity) { }

        float range;

        public override void Enter()
        {
            base.Enter();
            if (owner.Tribe.Name == "Cell")
            {
                Debug.Log($"{owner.name} state is Attack.");
            }
            range = owner.Tribe.Follow.ThresholdToFollow * 2;
        }
        public override void Execute()
        {
            if (!IsStatusComplete() || ElapsedTimeSinseStateStart > owner.Tribe.Idle.IdleTime)
            {
                return;
            }
            owner.GizmoObj.SetGizmo(range);
            // Move cell to target enemy cells.
            var targetDir = owner.TargetEnemyCell.transform.position - owner.transform.position;
            owner.Move(targetDir.normalized);
        }
        public override void Exit() { }
        bool IsStatusComplete()
        {
            if (!owner.IsStatusComplete()) return false;
            // Whether the distance between leader and me is greater than the specified distance.
            if(owner.MyLeaderTransform == null)
            {
                return false;
            }
            var diff = owner.MyLeaderTransform.position - owner.transform.position;
            if (diff.magnitude > range)
            {
                owner.ChangeState(CellState.Follow);
                return false;
            }
            // 指定範囲から敵が抜け出した場合,視認可能範囲を過ぎた時,違う敵を探す. 敵を検知出来なかったらwander stateへ遷移する
            var isEnemyLost = owner.DistanceToTargetCell > owner.Tribe.Sensor.PursueRange;
            var isEnemyIsNull = owner.TargetEnemyCell == null;
            if (isEnemyLost || isEnemyIsNull)
            {
                if (!owner.TryNeablyEnemyDetection())
                {
                    owner.ChangeState(CellState.Wander);
                    return false;
                }
            }
            return true;
        }
    }
}