using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CellsEcosystem
{
    /// <summary>
    /// state machine.
    /// </summary>
    public class GameStateMachine<TEnum> where TEnum : Enum
    {
        public enum When
        {
            Enter,
            Execute,
            Exit,
        }

        public TEnum CurrentState => currentState;
        public TEnum PreviousState => previousState;

        public event Action<TEnum> OnStateEnter = s => { Debug.Log($"Enter {s}"); };
        public event Action<TEnum> OnStateStay = s => { };
        public event Action<TEnum> OnStateExit = s => { Debug.Log($"Exit {s}"); };

        TEnum currentState;
        TEnum previousState;

        /// <summary>
        /// 既にEnterを呼んだ
        /// </summary>
        bool isCalledEnter;

        /// <summary>
        /// この関数がキックされる度に登録されたイベントをコールする。
        /// </summary>
        /// <remarks>
        /// Updateはインスタンスを持つMonoBehaviourで行う。
        /// </remarks>
        public void Update()
        {
            if (!isCalledEnter)
            {
                OnStateEnter?.Invoke(CurrentState);
                isCalledEnter = true;
            }
            else
            {
                OnStateStay?.Invoke(CurrentState);
            }
        }

        public void ChangeState(TEnum next)
        {
            if (isCalledEnter)
            {
                isCalledEnter = false;
                OnStateExit?.Invoke(CurrentState);
                previousState = CurrentState;
            }
            currentState = next;
        }

        public void SubscribeStateEvent(When when, Action<TEnum> action)
        {
            switch (when)
            {
                case When.Enter:
                    OnStateEnter += action;
                    break;
                case When.Execute:
                    OnStateStay += action;
                    break;
                case When.Exit:
                    OnStateExit += action;
                    break;
            }
        }

        public void DisposeStateEvent(When when, Action<TEnum> action)
        {
            switch (when)
            {
                case When.Enter:
                    OnStateEnter -= action;
                    break;
                case When.Execute:
                    OnStateStay -= action;
                    break;
                case When.Exit:
                    OnStateExit -= action;
                    break;
            }
        }
    }
}
