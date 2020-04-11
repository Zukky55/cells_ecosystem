using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CellsEcosystem
{
    /// BGMのID
    public enum BGM_ID
    {
        Title = 0,
        InStage = 1,

        None = 0xffff,
    }

    /// <summary>
    /// SEのID
    /// </summary>
    public enum SE_ID
    {
        /// <summary>接触</summary>
        Contact = 0,
        /// <summary>生まれる.</summary>
        Born = 1,
        /// <summary>死ぬ.</summary>
        Dead = 2,

        None = 0xffff,
    }

    /// <summary>
    /// ミキサーのグループ
    /// </summary>
    public enum MixerGroups
    {
        Master = 0,
        BGM,
        SE,
        Voice,

        Cell,
        Atom,
    }
    /// <summary>
    /// BGM,SEの再生や音量調整等のオーディオ全般を管理。
    /// </summary>
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [Serializable]
        public class Param
        {
            [SerializeField]
            float defaultFadeTime = 0.2f;

            public float DefaultFadeTime => defaultFadeTime;
        }



        [SerializeField]
        AudioSource[] sources_BGM;

        [SerializeField]
        AudioSource[] sources_2d_SE;

        [SerializeField]
        AudioSource[] sources_3d_SE;

        [SerializeField]
        SoundAssets soundAssets;

        [SerializeField]
        AudioMixer audioMixer;

        [SerializeField]
        Param param;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public float GetVolumeByLinear(MixerGroups group)
        {
            audioMixer.GetFloat(group.ToString(), out var decibel);
            return Mathf.Pow(10f, decibel / 20f);
        }

        /// <summary>
        /// グループごとにボリューム設定
        /// </summary>
        /// <param name="group"></param>
        /// <param name="volume"></param>
        public void SetVolume(MixerGroups group, float volume)
        {
            var decibel = 20f * Mathf.Log(volume);
            if (float.IsNegativeInfinity(decibel))
            {
                decibel = -96f;
            }
            audioMixer.SetFloat(group.ToString(), decibel);
        }

        /// <summary>
        /// ボリュームのアニメーション。
        /// </summary>
        /// <param name="source">対象のソース</param>
        /// <param name="target">アニメーション後の値</param>
        /// <param name="time">アニメーション時間</param>
        /// <param name="isStop">アニメーション後再生停止する</param>
        /// <returns></returns>
        IEnumerator FadeCoroutine(AudioSource source, float target, float time, bool isStop)
        {
            var diff = target - source.volume;
            var diffTime = diff / time;
            var timer = time;

            while (timer > 0)
            {
                var deltaTime = Time.unscaledDeltaTime;
                timer -= deltaTime;
                source.volume += diffTime * deltaTime;
                yield return null;
            }
            if (isStop)
            {
                source.Stop();
            }
        }

        /// <summary>
        /// 再生
        /// </summary>
        /// <param name="source">対象</param>
        /// <param name="clip">曲</param>
        /// <param name="volume">音量</param>
        /// <param name="isLoop">ループする</param>
        void Play(AudioSource source, AudioClip clip, float volume, bool isLoop = false)
        {
            source.clip = clip;
            source.loop = isLoop;
            source.volume = volume;
            source.Play();
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fadeTime">曲の遷移時間</param>
        public void PlayBGM(BGM_ID id, float fadeTime = -1f)
        {
            if (sources_BGM == null) return;
            var volume = 0f;
            fadeTime = fadeTime < volume ? param.DefaultFadeTime : fadeTime;

            var playingSource = sources_BGM.FirstOrDefault(s => s.isPlaying);
            if (playingSource != null)
            {
                StartCoroutine(FadeCoroutine(playingSource, volume, fadeTime, true));
            }

            volume = 1f;
            var unuseSource = sources_BGM.FirstOrDefault(s => !s.isPlaying);
            if (unuseSource == null) return;
            Play(unuseSource, soundAssets.BGMAssets.First(tag => tag.ClipTag.Equals(id)).Clip, 0, true);
            StartCoroutine(FadeCoroutine(unuseSource, volume, fadeTime, false));
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        /// <param name="outTime">フェードアウト時間</param>
        public void StopBGM(float outTime = 1f)
        {
            if (sources_BGM == null) return;

            var volume = 0f;
            var playingSource = sources_BGM.FirstOrDefault(s => s.isPlaying);
            if (playingSource != null)
            {
                StartCoroutine(FadeCoroutine(playingSource, volume, outTime, true));
            }
        }

        /// <summary>
        /// 2DのSE再生
        /// </summary>
        /// <param name="id"></param>
        public void PlaySE_2D(SE_ID id)
        {
            if (id == SE_ID.None)
            {
                return;
            }

            var volume = 1f;
            if (sources_2d_SE == null) return;
            var source = sources_2d_SE.FirstOrDefault(s => s.isPlaying == false);
            if (source != null)
            {
                Play(source, soundAssets.SEAssets.First(tag => tag.ClipTag.Equals(id)).Clip, volume);
            }
        }

        /// <summary>
        /// 3DサラウンドのSE再生
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="position">再生座標</param>
        public void PlaySE_3D(SE_ID id, Vector3 position)
        {
            if (id == SE_ID.None)
            {
                return;
            }

            var volume = 1f;
            if (sources_3d_SE == null) return;
            var source = sources_3d_SE.FirstOrDefault(s => s.isPlaying == false);
            if (source != null)
            {
                source.transform.position = position;
                Play(source, soundAssets.SEAssets.First(tag => tag.ClipTag.Equals(id)).Clip, volume);
            }
        }

        /// <summary>
        /// SE停止
        /// </summary>
        public void StopSE()
        {
            if (sources_2d_SE == null || sources_3d_SE == null) return;
            foreach (var s in sources_2d_SE)
            {
                if (s.isPlaying)
                {
                    s.Stop();
                }
            }
            foreach (var s in sources_3d_SE)
            {
                if (s.isPlaying)
                {
                    s.Stop();
                }
            }
        }
    }
}
