using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CellsEcosystem
{
    public abstract class StatefulObjectBase<TTarget, TEnum> : MonoBehaviour
        where TTarget : class 
        where TEnum : System.IConvertible
    {
        protected List<State<TTarget, TEnum>> stateList = new List<State<TTarget, TEnum>>();
        protected StateMachine stateMachine = new StateMachine();

        public virtual void ChangeState(TEnum nextState)
        {
            if (stateMachine == null)
            {
                return;
            }
            //Debug.Log($"{gameObject.name} to next state is  {nextState.ToString()} state.");
            stateMachine.ChangeState(stateList.Find(state => state.Identity.ToInt32(null) == nextState.ToInt32(null)));
        }

        public virtual bool IsCurrentState(TEnum identity)
        {
            if (stateMachine == null)
            {
                return false;
            }
            return stateMachine.CurrentState.Identity.Equals(identity);
        }

        protected virtual void Update()
        {
            if (stateMachine != null)
            {
                stateMachine.Update();
            }
        }

        /// <summary>
        /// 使用するStateMachine.
        /// </summary>
        protected class StateMachine
        {
            State<TTarget, TEnum> currentState;

            public StateMachine()
            {
                currentState = null;
            }

            public State<TTarget, TEnum> CurrentState
            {
                get
                {
                    return currentState;
                }
            }

            public void ChangeState(State<TTarget, TEnum> state)
            {
                if (currentState != null)
                {
                    currentState.Exit();
                }
                currentState = state;
                currentState.Enter();
            }

            public void Update()
            {
                if (currentState != null)
                {
                    currentState.Execute();
                }
            }
        }
    }
}