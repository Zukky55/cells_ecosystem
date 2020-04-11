using UnityEngine;
using System.Collections;
using System;

namespace CellsEcosystem
{
    public class State<T, TEnum>
    {
        /// <summary>Return elapsed time since state start.</summary>
        public float ElapsedTimeSinseStateStart => Time.time - elapsedTime;
        /// <summary>the identity of state.</summary>
        public TEnum Identity => identity;
        /// <summary>このステートを使用するインスタンス</summary>
        public T Onwer => owner;

        /// <summary>このステートを使用するインスタンス</summary>
        protected T owner;
        /// <summary>the identity of state.</summary>
        protected TEnum identity;
        /// <summar>Elapsed time since state start.</summary>
        float elapsedTime;

        /// <summary>
        /// State constructor
        /// </summary>
        /// <param name="owner"></param>
        public State(T owner, TEnum identity)
        {
            this.owner = owner;
            this.identity = identity;
        }

        /// <summary>
        /// このステートに遷移する時に一度だけ呼ばれる
        /// </summary>
        public virtual void Enter()
        {
            elapsedTime = Time.time;
        }
        /// <summary>
        /// このステートで有る間、毎フレーム呼ばれる
        /// </summary>
        public virtual void Execute() { }
        /// <summary>
        /// このステートから他のステートに遷移する時に一度だけ呼ばれる
        /// </summary>
        public virtual void Exit() { }
    }
}
