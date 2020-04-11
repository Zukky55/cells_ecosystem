using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CellsEcosystem
{
    /// <summary>
    /// The ToDie state behaivor.
    /// </summary>
    public class StateToDie : State<Cell, CellState>
    {
        public StateToDie(Cell owner, CellState identity) : base(owner, identity) { }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Execute()
        {
            base.Execute();
            if (ElapsedTimeSinseStateStart < owner.Tribe.Physical.DestroyDelayTime)
            {
                if (owner.IsLeader)
                {
                    // If there is a survival other than myself, inherit the post to that cell.
                    if (Ecosystem.DetectTribe(owner.Tribe.Name).Any(cell => !cell.IsPlayer))
                    {
                        owner.DecideNextLeader();
                    }
                    else
                    {
                        Ecosystem.Extinction(owner.Tribe);
                    }
                }
                var obj = Resources.Load("Explosion") as GameObject;
                UnityEngine.Object.Destroy(GameObject.Instantiate(obj, owner.transform.position, Quaternion.identity), 1f);
                UnityEngine.Object.Destroy(owner.gameObject);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}