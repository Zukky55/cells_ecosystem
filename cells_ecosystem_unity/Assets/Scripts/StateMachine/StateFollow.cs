using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Follow state behaivor.
    /// </summary>
    public class StateFollow : State<Cell, CellState>
    {
        public StateFollow(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
            owner.GizmoObj.SetGizmo(owner.Tribe.Follow.ThresholdToFollow);
        }
        public override void Execute()
        {
            if (!IsStatusComplete() && ElapsedTimeSinseStateStart > owner.Tribe.Idle.IdleTime)
            {
                return;
            }
            // If own leader is not found, then end process.
            if (owner.MyLeaderTransform == null)
            {
                //Debug.Log($"{owner.Tribe.Name}のリーダーみつかんないっす");
                return;
            }

            // Head towards the target.
            var targetVec = owner.MyLeaderTransform.position - owner.transform.position;
            if (owner.Tribe.Follow.ThresholdToFollow < targetVec.magnitude)
            {
                owner.Move(targetVec.normalized);
            }
            else //  If the distance between the owner and the target is close , apply the brakes.
            {
                owner.Brake();
            }
        }
        public override void Exit() { }

        bool IsStatusComplete()
        {
            if (!owner.IsStatusComplete()) return false;
            // If own leader is not found, then end process.
            if (owner.MyLeaderTransform == null)
            {
                Debug.Log($"{owner.Tribe.Name}のリーダーみつかんないっす");
                return false;
            }
            // Whether the distance between leader and me is greater than the specified distance.
            var diff = owner.MyLeaderTransform.position - owner.transform.position;
            var isGreaterThanTheDistance = diff.magnitude > owner.Tribe.Follow.ThresholdToFollow * 2;
            // It changes to attack state when it detects surrounding enemies.
            if (owner.TryNeablyEnemyDetection() && !isGreaterThanTheDistance)
            {
                owner.ChangeState(CellState.Attack);
                return false;
            }



            return true;
        }
    }
}