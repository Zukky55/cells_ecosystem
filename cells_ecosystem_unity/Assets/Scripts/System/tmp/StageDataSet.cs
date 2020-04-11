using UnityEngine;
using System;
using System.Collections.Generic;

namespace CellsEcosystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "StageDataSet", menuName = "ScriptableObject/StageDataSet")]
    public class StageDataSet : ScriptableObject
    {
        [SerializeField]
        List<Pattern> patterns = new List<Pattern>();



        /// <summary>
        /// ステージのレベルデザインパターン
        /// </summary>
        [Serializable]
        public class Pattern
        {
            /// <summary>
            /// ステージ上のNPC種族の配置パターン
            /// </summary>
            public GameObject EnemiesPattern;

            public AtomParameter AtomParam;
        }

        /// <summary>
        /// <see cref="Atom"/>の生成管理等に関係するパラメータ
        /// </summary>
        [Serializable]
        public class AtomParameter
        {
            /// <summary>
            /// Atom生成座標
            /// </summary>
            public List<Transform> spawnNodes = new List<Transform>();

            /// <summary>
            /// 同時生成上限
            /// </summary>
            public int ConcurrentSpawnLimit = 100;

            /// <summary>
            /// 生成時間間隔
            /// </summary>
            public float SpawnTimeInterval = .5f;

            ///// <summary>
            ///// ランダム生成座標範囲
            ///// </summary>
            //public int MaxSpawnRange = 75;
        }
    }
}