using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace CellsEcosystem
{
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        public enum Scene
        {
            Title,
            Stage,

            Invalid = 0xffff,
        }

        [SerializeField]
        Image screenImage = null;

        [SerializeField]
        Color fadingColor = Color.black;

        /// <summary>
        /// フェード時に使用するCanvas
        /// </summary>
        [SerializeField]
        float defaultFadeTime = 0.4f;

        /// <summary>
        /// 現在アクティブなメインシーン
        /// </summary>
        public Scene Current { get; set; } = Scene.Title;

        /// <summary>
        /// フェードイン終了時のコールバック
        /// </summary>
        public event Action<Scene> OnCompletedFadeIn = (Scene) => { };

        public void ChangeScene(Scene scene, float fadeTime = -1f)
        {
            if (fadeTime < 0f) { fadeTime = defaultFadeTime; }
            StartCoroutine(ChangeSceneCoroutine(scene, fadeTime));
        }

        public void FadeIn(float fadeTime = -1f)
        {
            if (fadeTime < 0f) { fadeTime = defaultFadeTime; }
            StartCoroutine(FadeInAsync(fadeTime));
        }

        public void FadeOut(float fadeTime = -1f)
        {
            if (fadeTime < 0f) { fadeTime = defaultFadeTime; }
            StartCoroutine(FadeOutAsync(fadeTime));
        }

        IEnumerator ChangeSceneCoroutine(Scene scene, float fadeTime)
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;

            if (eventSystem == null)
            {
                yield break;
            }

            yield return FadeOutAsync(fadeTime);

            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene);

            // scene切り替わった段階で更新
            Current = scene; 

            yield return FadeInAsync(fadeTime);

            eventSystem = UnityEngine.EventSystems.EventSystem.current;
            eventSystem.enabled = true;
        }

        /// <summary>
        /// フェードインのコルーチン
        /// </summary>
        IEnumerator FadeInAsync(float fadeTime)
        {
            var startTime = Time.timeSinceLevelLoad;
            var elapsedTime = 0f;
            var from = 1f;
            var to = 0f;
            var t = 0f;
            var alpha = 0f;


            while (t < 1f)
            {
                elapsedTime = Time.timeSinceLevelLoad - startTime;
                t = elapsedTime / fadeTime;
                alpha = Mathf.Lerp(from, to, t);
                screenImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, alpha);
                yield return null;
            }
            screenImage.enabled = false;
            OnCompletedFadeIn?.Invoke(Current);
        }

        /// <summary>
        /// フェードアウトのコルーチン
        /// </summary>
        IEnumerator FadeOutAsync(float fadeTime)
        {
            var startTime = Time.timeSinceLevelLoad;
            var elapsedTime = 0f;
            var from = 0f;
            var to = 1f;
            var t = 0f;
            var alpha = 1f;

            screenImage.enabled = true;

            while (t < 1f)
            {
                elapsedTime = Time.timeSinceLevelLoad - startTime;
                t = elapsedTime / fadeTime;
                alpha = Mathf.Lerp(from, to, t);
                screenImage.color = new Color(fadingColor.r, fadingColor.g, fadingColor.b, alpha);
                yield return null;
            }
        }
    }
}