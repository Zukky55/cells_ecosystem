using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellsEcosystem
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "CellsData", menuName = "ScriptableObject/Cell/CellsData")]
    public partial class CellsData : ScriptableObject
    {
        public List<TribeStatus> Tribes { get => tribes; set => tribes = value; }
        /// <summary></summary>
        [SerializeField] List<TribeStatus> tribes;
    }

    #region Enumerables
    public enum CellCharacter
    {
        /// <summary>従順</summary>
        Obedience,
        /// <summary>臆病</summary>
        Cowardice,
        /// <summary>変人</summary>
        Weirdo,
        /// <summary>凶暴</summary>
        Violent,
    }
    /// <summary>
    /// Cellの種類
    /// </summary>
    public enum CellKind
    {
        /// <summary>族長</summary>
        Leader,
        /// <summary>部族</summary>
        Tribe,
        /// <summary>プレイヤー自身</summary>
        Player,
    }
    /// <summary>
    /// 種族とプレイヤーの関係性
    /// </summary>
    public enum TribalType
    {
        /// <summary>敵対的</summary>
        Hostile,
        /// <summary>中立</summary>
        Neutral,
        /// <summary>友好的</summary>
        Friendly,
    }
    #endregion
}
