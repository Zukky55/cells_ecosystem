using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The Wander state behaivor.
    /// </summary>
    public class StateWander : State<Cell, CellState>
    {
        const float CHANGE_DIRECTION_TIME = 3f;
        const float THETA = 360f;
        const float CHANGE_STATE_TIME = 10f;

        Vector3 direction;

        public StateWander(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Execute()
        {
            if (!owner.IsStatusComplete()) return;
            if (ElapsedTimeSinseStateStart < CHANGE_STATE_TIME) owner.ChangeState(CellState.Follow);
            if (ElapsedTimeSinseStateStart < CHANGE_DIRECTION_TIME) direction = GetRandomDirection();
            owner.Move(direction.normalized);
        }
        public override void Exit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Vector3 GetRandomDirection()
        {
            var beforeAngle = owner.transform.localEulerAngles;
            var afterAngle = new Vector3(beforeAngle.x, Random.Range(0, THETA), beforeAngle.z);
            return afterAngle;
        }
    }
}