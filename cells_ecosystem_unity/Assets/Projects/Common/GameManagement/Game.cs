using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CellsEcosystem
{
    public class Game : SingletonMonoBehaviour<Game>
    {
        public enum State
        {
            Title,
            InitGame,
            InGame,
            Result,
            Pause,
            Invalid = 0xffff,
        }

        GameStateMachine<State> stateMachine = new GameStateMachine<State>();
        bool isInitialized = false;

        public override void OnInitialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (!Instance.isInitialized) return;

            stateMachine.Update();
        }

        public static void Initialize()
        {
            if (Instance.isInitialized) return;

            Instance.stateMachine.ChangeState(State.Title);
            Instance.isInitialized = true;
        }

        public static void StartGame()
        {
            Instance.stateMachine.ChangeState(State.InitGame);
        }

        public static void Pause()
        {
            Instance.stateMachine.ChangeState(State.Pause);
        }

        public static void Result()
        {
            Instance.stateMachine.ChangeState(State.Result);
        }

        public static void SubscribeStateEvent(GameStateMachine<State>.When when, Action<State> action) => Instance.stateMachine.SubscribeStateEvent(when, action);
        public static void DisposeStateEvent(GameStateMachine<State>.When when, Action<State> action) => Instance.stateMachine.DisposeStateEvent(when, action);
    }
}
