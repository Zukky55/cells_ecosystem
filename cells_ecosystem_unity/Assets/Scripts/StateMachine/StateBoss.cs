using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    /// <summary>
    /// The ToDie state behaivor.
    /// </summary>
    public class StateBoss : State<Cell, CellState>
    {
        const float CHANGE_DIRECTION_TIME = 5f;
        const float THETA = 360f;
        const float CHANGE_STATE_TIME = 5f;
        Vector3 direction;
        Ecosystem ecosystem;
        public StateBoss(Cell owner, CellState identity) : base(owner, identity) { }

        public override void Enter()
        {
            base.Enter();
            ecosystem = Ecosystem.Instance;
            // TODO: ステータスの差し替えにする
            //    owner.Tribe.Name = "Boss";
            //    owner.gameObject.name = "Boss";
            //    owner.Tribe.Speed *= 0.75f;
        }
        public override void Execute()
        {
            if (ElapsedTimeSinseStateStart < CHANGE_DIRECTION_TIME) direction = GetRandomDirection();

            if (owner.TryNeablyEnemyDetection())
            {
                var dir = owner.TargetEnemyCell.transform.position - owner.transform.position;
            //Debug.Log("敵のほう");
                owner.Move(dir.normalized);
                return;
            }
            var vec = direction - owner.transform.position;
            //Debug.Log("せるの方");
            owner.Move(vec.normalized);
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
            var vec = ecosystem.CellsOntheStage[Random.Range(0, ecosystem.CellsOntheStage.Count())];

            return vec.transform.position;
        }
    }
}