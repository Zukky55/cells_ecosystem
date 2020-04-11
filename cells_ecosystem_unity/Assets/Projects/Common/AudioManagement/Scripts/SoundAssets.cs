using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellsEcosystem
{

    public interface ISoundPlayer
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Audio/SoundAsset", fileName = "ScriptableObject/SoundAsset")]
    public class SoundAssets : ScriptableObject
    {
        [SerializeField]
        BGMAsset[] bgmAssets;
        [SerializeField]
        SEAsset[] seAssets;

        public SEAsset[] SEAssets { get => seAssets; set => seAssets = value; }
        public BGMAsset[] BGMAssets { get => bgmAssets; set => bgmAssets = value; }

        [Serializable]
        public class SEAsset : SoundAsset<SE_ID> { }
        [Serializable]
        public class BGMAsset : SoundAsset<BGM_ID> { }

        [Serializable]
        public class SoundAsset<TEnum> where TEnum : Enum
        {
            public TEnum ClipTag => clipTag;
            public AudioClip Clip => clip;

            [SerializeField] TEnum clipTag;
            [SerializeField] AudioClip clip;
        }
    }
}
