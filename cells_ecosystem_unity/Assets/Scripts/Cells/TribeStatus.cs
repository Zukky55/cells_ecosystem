using System;
using UnityEngine;

namespace CellsEcosystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "TribeStatus", menuName = "ScriptableObject/Cell/TribeStatus")]
    public class TribeStatus : ScriptableObject
    {
        #region Params
        public class BasicParam
        {
            //public float Speed { get => Input.GetKey(KeyCode.LeftShift) ? speed * dushAcceleration : speed; set => speed = value; }

            /// <summary>種族の体力</summary>
            public int Life;
            /// <summary>種族の攻撃力</summary>
            public int Attack;
            /// <summary>種族の質量</summary>
            public int Mass;
            /// <summary>種族の移動速度(Shift keyが押されている間はdushAccelerationが乗算される)</summary>
            public float Speed;
            /// <summary>ダッシュ時の加速度</summary>
            public float DushAcceleration;
            /// <summary>種族の速度</summary>
            public float TurnInterpolate;
        }
        public class PhysicalParam
        {
            /// <summary>セルを弾き飛ばす高さ</summary>
            public float HeightForce;
            /// <summary>セルを飛ばす距離</summary>
            public float SphereForce;
            /// <summary>ブレーキを掛ける強さ(初期値0. 95f.)</summary>
            public float BrakeStrength = 0.9f;
            /// <summary>飛んでから死ぬ迄の遅延時間</summary>
            public float DestroyDelayTime; // TODO: これの扱い決める。多分種族に依存しないgeneralParamみたいなの作る
        }
        public class FollowParam
        {
            /// <summary>リーダーに追従するしきい値(判定対象は自身と対象{leader}ノルムの二乗.sqrMagnitude)</summary>
            public float ThresholdToFollow;
        }
        public class SensorParam
        {
            /// <summary>敵を検知する半径</summary>
            public float DetectEnemySensorRange;
            /// <summary>対象を追跡するボーダーライン</summary>
            public float PursueRange;
        }
        public class IdleParam
        {
            /// <summary>Time to wait on Idle state.</summary>
            public float IdleTime = 3f;
        }
        #endregion


        #region Variables
        [SerializeField]
        string tribeName;
        [SerializeField]
        Color color;
        [SerializeField]
        Ecosystem.Rank rank;
        [SerializeField]
        Mesh mesh;

        BasicParam basic = new BasicParam();
        PhysicalParam physical = new PhysicalParam();
        FollowParam follow = new FollowParam();
        SensorParam sensor = new SensorParam();
        IdleParam idle = new IdleParam();

        /// <summary>
        /// 種族名
        /// </summary>
        public string Name => tribeName;

        /// <summary>
        /// 種族の色
        /// </summary>
        public Color Color => color;

        /// <summary>
        /// 種族のランク
        /// </summary>
        public Ecosystem.Rank Rank => rank;

        /// <summary>
        /// Mesh
        /// </summary>
        public Mesh Mesh => mesh;

        /// <summary>
        /// 基本的なパラメータ
        /// </summary>
        public BasicParam Basic => basic;

        /// <summary>
        /// 物理で使うパラメータ
        /// </summary>
        public PhysicalParam Physical => physical;

        /// <summary>
        /// <see cref="StateFollow"/>用パラメータ
        /// </summary>
        public FollowParam Follow => follow;

        /// <summary>
        /// <see cref="StateIdle"/>用パラメータ
        /// </summary>
        public IdleParam Idle => idle;

        /// <summary>
        /// センサー用パラメータ
        /// </summary>
        public SensorParam Sensor => sensor;
        #endregion
    }
}
