using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CellsEcosystem
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField]
        float bgmFadeDuration;

        [SerializeField]
        ButtonUI buttonUI;

        [SerializeField]
        TitleUI titleUI;

        private void OnEnable()
        {
            SceneManager.Instance.OnCompletedFadeIn += OnCompletedFadeIn;
        }

        /// <summary>
        /// When completed fade of scene then kick this callback.
        /// </summary>
        /// <param name="scene">Current scene</param>
        private async void OnCompletedFadeIn(SceneManager.Scene scene)
        {
            if (scene != SceneManager.Scene.Title) return;

            // タイトル再生->bgm 再生->ボタン表示
            await titleUI.PlayAsync(TitleUI.Tag.Init);
            AudioManager.Instance.PlayBGM(BGM_ID.Title, bgmFadeDuration);
            
            await buttonUI.PlayAsync(ButtonUI.Tag.Init);
            buttonUI.button.interactable = true;
        }

        private void Start()
        {
            buttonUI.button.interactable = false;
            SceneManager.Instance.FadeIn();
        }

        /// <summary>
        /// For button events.
        /// </summary>
        public void StartGameFromButton()
        {
            buttonUI.button.interactable = false;
            SceneManager.Instance.ChangeScene(SceneManager.Scene.Stage);
        }
    }
}