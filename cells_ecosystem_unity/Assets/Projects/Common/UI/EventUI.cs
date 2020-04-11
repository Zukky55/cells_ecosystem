using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;

namespace CellsEcosystem
{
    public abstract class EventUI<TEnum> : MonoBehaviour, IPlayableUIAnimation<TEnum> where TEnum : Enum
    {
        [SerializeField]
        [Header("再生させたアニメーションが再生完了した時")]
        public UnityEvent OnEndTargetAnimation;

        protected Animator animator => GetComponent<Animator>();


        public virtual async UniTask PlayAsync(TEnum tag)
        {
            Play(tag.ToString());
            await StartCoroutine(CallbackEndAnimationAsync());
        }

        public virtual void Play(TEnum tag)
        {
            Play(tag.ToString());
        }

        public virtual void Play(string tag)
        {
            animator.Play(tag);
            StartCoroutine(CallbackEndAnimationAsync());
        }

        public virtual void SetBool(TEnum tag, bool flag)
        {
            animator.SetBool(tag.ToString(), flag);
        }

        public virtual void SetFloat(TEnum tag, float param)
        {
            animator.SetFloat(tag.ToString(), param);
        }

        public virtual void SetTrigger(TEnum tag)
        {
            animator.SetTrigger(tag.ToString());
        }

        IEnumerator CallbackEndAnimationAsync()
        {
            // 現在のアニメーションが終わるまで遅延
            animator.Update(0);
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);

            yield return new WaitForSeconds(clipInfo[0].clip.length);
            OnEndTargetAnimation?.Invoke();
        }
    }
}