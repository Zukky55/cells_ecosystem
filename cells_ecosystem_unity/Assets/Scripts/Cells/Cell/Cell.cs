using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    public partial class Cell : StatefulObjectBase<Cell, CellState>
    {
        #region Properties
        /// <summary>tribe information of cell</summary>
        public TribeStatus Tribe
        {
            get => tribe;
            set => tribe = value;
        }
        /// <summary>Whether i'm grounded</summary>
        public bool IsGrounded
        {
            get => flags.HasFlag(CellFlags.IsGrounded);
            set
            {
                if (value)
                {
                    flags |= CellFlags.IsGrounded;
                }
                else
                {
                    flags &= ~CellFlags.IsGrounded;
                }
            }
        }
        /// <summary>Whether i'm Leader</summary>
        public bool IsLeader
        {
            get => flags.HasFlag(CellFlags.IsLeader);
            set
            {
                if (value)
                {
                    flags |= CellFlags.IsLeader;
                }
                else
                {
                    flags &= ~CellFlags.IsLeader;
                }
            }
        }
        /// <summary>Whether i'm Player</summary>
        public bool IsPlayer
        {
            get => flags.HasFlag(CellFlags.IsPlayer);
            set
            {
                if (value)
                {
                    flags |= CellFlags.IsPlayer;
                }
                else
                {
                    flags &= ~CellFlags.IsPlayer;
                }
            }
        }
        /// <summary>Whether i'm Player</summary>
        public bool IsBoss
        {
            get => flags.HasFlag(CellFlags.IsBoss);
            set
            {
                if (value)
                {
                    flags |= CellFlags.IsBoss;
                }
                else
                {
                    flags &= ~CellFlags.IsBoss;
                }
            }
        }
        /// <summary>Sound assets</summary>
        public SoundAssets SAssets => soundAssets;
        /// <summary>Rigidbody</summary>
        public Rigidbody Rb { get; private set; }
        /// <summary>The Audio source</summary>
        public AudioSource ASource { get; set; }
        /// <summary>Return transform of my leader.</summary>
        //public Transform MyLeaderTransform => leaderTransform ? leaderTransform :
        //leaderTransform = ecosystem.DetectTribeOrDefault(Tribe.Name).Single(cell => cell.IsLeader).transform;
        public Transform MyLeaderTransform
        {
            get
            {
                if (IsLeader) return transform;
                if (leaderTransform != null) return leaderTransform;

                var cells = Ecosystem.DetectTribe(Tribe.Name);
                var leader = cells.SingleOrDefault(cell => cell.IsLeader);
                leaderTransform = leader?.transform;
                return leaderTransform;
            }
        }
        /// <summary>追跡する対象の敵セル</summary>
        public Cell TargetEnemyCell { get => targetEnemyCell; set => targetEnemyCell = value; }
        public float DistanceToTargetCell
        // => (TargetEnemyCell?.transform.position - transform.position).Value.sqrMagnitude;
        {
            get
            {
                if (TargetEnemyCell != null)
                {
                    return (TargetEnemyCell.transform.position - transform.position).sqrMagnitude;
                }
                return 0f;
                //var res = (TargetEnemyCell?.transform.position - transform.position).Value.sqrMagnitude;
                //Debug.Log($"result is {res}");
                //return res;
            }
        }
        /// <summary>Gizmo object.</summary>
        public GizmoObject GizmoObj { get; private set; }
        #endregion
        #region Variables
        [SerializeField] TribeStatus tribe;
        /// <summary>種族タイプ</summary>
        [SerializeField] protected TribalType tribalType;
        /// <summary>Current state.</summary>
        [SerializeField] protected string currentState;
        /// <summary>test code : first attach flags.</summary>
        [SerializeField] CellFlags testFirstAttachFlag; // check
        /// <summary>the sound assets.</summary>
        [SerializeField] SoundAssets soundAssets;
        /// <summary>質量の増加率</summary>
        [SerializeField] float increaseRate = 1.5f;
        /// <summary>質量の減少率</summary>
        [SerializeField] float decreaseRate = .5f;

        TribeStatus useStatus;
        Material material;
        /// <summary>flag.</summary>
        protected CellFlags flags; // check
        /// <summary>the target enemy cell.</summary>
        protected Cell targetEnemyCell;
        protected Atom targetAtom;
        /// <summary>tribe.</summary>
        /// <summary>leader's transform.</summary>
        protected Transform leaderTransform;
        #endregion
        #region Methods
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize()
        {
            Rb = GetComponent<Rigidbody>();
            ASource = GetComponent<AudioSource>();
            //Tribe = testTribe; // check
            if (testFirstAttachFlag == CellFlags.IsPlayer)
            {
                IsPlayer = true;
                IsLeader = true;
            }
            if (testFirstAttachFlag == CellFlags.IsLeader)
            {
                IsLeader = true;
            }

            if (Tribe == null)
            {
                Tribe = Ecosystem.Tribes.SingleOrDefault(t => t.Name == name);
            }
            Rb.mass = Tribe.Basic.Mass;
            Rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            material = GetComponent<MeshRenderer>().material;
            ColorChange(Tribe.Color);
            GizmoObj = GetComponentInChildren<GizmoObject>();
            GizmoObj.SetGizmo(360f, 0f, 1f, Tribe.Color); // check

            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            //TODO: 直す

            stateList.Add(new StateToDie(this, CellState.ToDie));
            stateList.Add(new StatePlayer(this, CellState.Player));
            stateList.Add(new StateAttack(this, CellState.Attack));
            stateList.Add(new StateFollow(this, CellState.Follow));
            stateList.Add(new StateLeader(this, CellState.Leader));
            stateList.Add(new StateEscape(this, CellState.Escape));
            stateList.Add(new StateWander(this, CellState.Wander));
            //stateList.Add(new StateIdle(this, CellState.Idle));
            stateList.Add(new StateBoss(this, CellState.Boss));
        }

        /// <summary>
        /// 指定Clipを再生
        /// </summary>
        /// <param name="targetClip">the specified audio clip.</param>
        void PlayShotSound(AudioClip targetClip) => ASource?.PlayOneShot(targetClip);
        /// <summary>
        /// 自分がリーダーの時,自分に一番近い種族のセルをリーダーに任命する.
        /// </summary>
        public void DecideNextLeader()
        {
            if (!IsLeader)
            {
                Debug.LogWarning($"I'm not leader of  {Tribe.Name}.");
                return;
            }
            var tribalCellsOtherThanPlayerCell = Ecosystem.DetectTribe(Tribe.Name).FindAll(cell => !cell.IsPlayer);
            if (tribalCellsOtherThanPlayerCell.Count == 0)
            {
                Debug.LogWarning("絶滅してるのに呼ばれてるよ");
                return;
            }

            var distance = (tribalCellsOtherThanPlayerCell[0].transform.position - transform.position).sqrMagnitude;
            var mostNeablyCell = tribalCellsOtherThanPlayerCell[0];
            foreach (var cell in tribalCellsOtherThanPlayerCell)
            {
                var dis = cell.transform.position - transform.position;
                if (distance > dis.sqrMagnitude)
                {
                    mostNeablyCell = cell;
                }
            }
            // Inherit the IsLeader.
            IsLeader = false;
            mostNeablyCell.IsLeader = true;
            // If the target is a PlayerCell. it'll also inherit IsPlayer.
            if (Ecosystem.PlayerCell == null)
            {

            }
            if (Tribe.Name == Ecosystem.PlayerCell.Tribe.Name)
            {
                IsPlayer = false;
                mostNeablyCell.IsPlayer = true;
            }
        }
        /// <summary>
        /// 引数分のダメージを受ける
        /// </summary>
        /// <param name="damage">与えられるダメージ</param>
        public void TakeDamage(int damage)
        {
            //Debug.Log($"{name} of {Tribe.Name} is taking {damage} damage.");
            Tribe.Basic.Life -= damage;
            if (Tribe.Basic.Life <= 0)
            {
                ChangeState(CellState.ToDie);
            }
        }
        /// <summary>
        /// 自分自身を当たった相手の反対方向へ弾き飛ばす
        /// </summary>
        public void BlowOffRandomPlace(Cell collidedCell)
        {
            var diff = (collidedCell.transform.position - transform.position).normalized;
            //var force = Vector3.up * Tribe.HeightForce + new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)) * Tribe.SphereForce;
            var force = Vector3.up * Tribe.Physical.HeightForce + -diff * Tribe.Physical.SphereForce;
            if (collidedCell.Tribe.Basic.Mass > Tribe.Basic.Mass) // If the target cell has a larger mass.
            {
                Rb.AddForce(force * decreaseRate);
            }
            else if (collidedCell.Tribe.Basic.Mass == Tribe.Basic.Mass) // If the mass is the same　
            {
                Rb.AddForce(force);
            }
            else // If the mass is greater than the target cell.
            {
                Rb.AddForce(force * increaseRate);
            }
        }
        /// <summary>
        /// セルと接触した時
        /// </summary>
        /// <param name="collidedCell">collided cell.</param>
        private void OnCollideCell(Cell collidedCell)
        {
            //collidedCell.GetComponent<MeshRenderer>().material.color = Color.white;
            // 接地していない,もしくは相手が自種族だった場合何もしない
            if (collidedCell.Tribe.Name == Tribe.Name || !IsGrounded)
            {
                return;
            }
            //PlayShotSound(SoundAssets.SoundKind.Cell, SoundAssets.SoundTag.Collide);
            collidedCell.TakeDamage(Tribe.Basic.Attack);
            collidedCell.BlowOffRandomPlace(this);
            BlowOffRandomPlace(collidedCell);
        }
        /// <summary>
        /// Move cells.  argument is Unit vector.
        /// </summary>
        /// <param name="targetDirection">Direction to move.</param>
        public void Move(Vector3 targetDirection, bool isFacingBack = false)
        {
            if (!gameObject)
            {
                return;
            }

            if (targetDirection != Vector3.zero && !isFacingBack)
            {
                transform.forward = Vector3.Slerp(transform.forward, targetDirection, Tribe.Basic.TurnInterpolate);
            }
            Rb.velocity = targetDirection * Tribe.Basic.Speed;
        }
        /// <summary>
        /// the brake.
        /// </summary>
        public void Brake()
        {
            if (Rb.velocity.sqrMagnitude > Vector3.zero.sqrMagnitude)
            {
                Rb.velocity *= Tribe.Physical.BrakeStrength;
            }
        }
        /// <summary>
        /// Change color of mesh of cell.
        /// </summary>
        /// <param name="color"></param>
        public void ColorChange(Color color) => material.color = color;
        /// <summary>
        /// animation event用
        /// </summary>
        public void ColorChange() => ColorChange(Tribe.Color);

        /// <summary>
        /// センサー半径内に敵セルを見つけたら一番近くにいるセルをメンバーに格納しtrueを返す.
        /// 敵を検知できなかったらメンバーにnullを入れてfalseを返す.
        /// </summary>
        /// <returns>Whether the enemy has been detected successfully.</returns>
        public bool TryNeablyEnemyDetection()
        {
            float heap = Tribe.Sensor.DetectEnemySensorRange;
            Cell target = null;
            var layerMask = LayerMask.GetMask("Cell");
            var detectedCollideCells =
                Physics.OverlapSphere(transform.position, Tribe.Sensor.DetectEnemySensorRange, layerMask);
            foreach (var obj in detectedCollideCells)
            {
                var distance = (obj.transform.position - transform.position).magnitude;
                var cell = obj.GetComponent<Cell>();
                if (heap > distance && cell.Tribe.Name != Tribe.Name)
                {
                    heap = distance;
                    target = obj.GetComponent<Cell>();
                }
            }
            if (target != null)
            {
                //Debug.Log($"{name} detect enemy cell.");
                TargetEnemyCell = target;
                return true;
            }
            //Debug.Log($"There is no cell around, return null.");
            return false;
        }
        public Atom TryNeablyAtomDetection()
        {
            float heap = 1000;
            Atom target = null;
            var layerMask = LayerMask.GetMask("Atom");
            var detectAtoms =
                Physics.OverlapSphere(transform.position, heap, layerMask);
            foreach (var obj in detectAtoms)
            {
                var distance = (obj.transform.position - transform.position).magnitude;
                if (heap > distance)
                {
                    heap = distance;
                    target = obj.GetComponent<Atom>();
                }
            }
            if (target != null)
            {
                return target;
            }
            return null;
        }
        /// <summary>
        /// Check status.  Transition the state according to the situation. 
        /// Return value is Whether cell have a post.
        /// </summary>
        /// <returns>Whether cell have a post.</returns>
        public bool IsStatusComplete()
        {
            // if owner is player then go to state Player.
            if (IsPlayer)
            {
                ChangeState(CellState.Player);
                return false;
            }
            // if owner is leader then go tu state Leader.
            else if (IsLeader)
            {
                ChangeState(CellState.Leader);
                return false;
            }
            else if (IsBoss)
            {
                ChangeState(CellState.Boss);
                return false;
            }

            return true;
        }
        #endregion
        #region Callbacks
        protected virtual void Awake()
        {
            Initialize();
        }
        private void Start()
        {

            ChangeState(CellState.Follow); // check. test statemachine.
        }
        protected override void Update()
        {
            base.Update();
            currentState = stateMachine?.CurrentState.ToString();
        }
        protected virtual void FixedUpdate()
        {

        }
        #endregion
    }
    #region Enumerables
    [Flags]
    public enum CellFlags
    {
        None = 0,
        /// <summary>地面に接地しているか</summary>
        IsGrounded = 1,
        /// <summary>族長かどうか</summary>
        IsLeader = 2,
        /// <summary>Player(操作しているセル)かどうか</summary>
        IsPlayer = 4,
        /// <summary>is the boss</summary>
        IsBoss = 8,
    }
    public enum CellState
    {
        Leader,
        Follow,
        Attack,
        Escape,
        ToDie,
        Player,
        Wander,
        Idle,
        Boss,
    }
    public enum AnimTag
    {
        Damage,
        ToDie,
    }
    #endregion
}
