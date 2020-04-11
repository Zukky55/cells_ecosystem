using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace CellsEcosystem
{
    #region Properties
    public class Ecosystem : SingletonMonoBehaviour<Ecosystem>
    {
        public static List<Atom> AtomsOnStage { get => Instance.atomsOnStage; set => Instance.atomsOnStage = value; }
        /// <summary>All tribes.</summary>
        public static IEnumerable<TribeStatus> Tribes { get => Instance.tribes; set => Instance.tribes = value; }
        /// <summary>Player cell on the stage.</summary>
        public static Cell PlayerCell => Instance.playerCell ? Instance.playerCell : Instance.playerCell = Instance.DetectPlayerCell();

        public int MapSquare => mapSquare;
        /// <summary>All cells on the stage.</summary>
        public List<Cell> CellsOntheStage => (FindObjectsOfType(typeof(Cell)) as Cell[]).ToList();
        /// <summary>Cells data.</summary>
        public CellsData MasterData => masterData;

        /// <summary>Whether there is own tribe other than the player.</summary>
        public bool IsPlayersTribeExists => DetectTribe(playerCell.Tribe.Name).Any();


        #endregion
        #region  Variables
        [SerializeField] CellsData masterData;
        [SerializeField] GameObject spawnPointsParent;
        [SerializeField] float atomGenerationInterval = .5f;
        [SerializeField] int atomsLimit = 100;
        [SerializeField] int mapSquare = 75;

        IEnumerable<TribeStatus> tribes;
        List<Atom> atomsOnStage = new List<Atom>();
        TribeStatus playersTribe;
        Cell playerCell;
        GameObject atom;
        float spawnHeight = 3;
        float sphereScalar = 3;
        /// <summary>種族セルを生成させる座標同士の最低間隔</summary>
        #endregion
        #region Methods
        /// <summary>
        /// 新しい種族を発見したらCellsDataに追加する
        /// </summary>
        void DetectNewTribes()
        {
            foreach (var cell in CellsOntheStage)
            {
                if (!masterData.Tribes.Any(c => c.Name == cell.Tribe.Name))
                {
                    masterData.Tribes.Add(cell.Tribe);
                }
            }
        }
        /// <summary>
        /// Return found cell of specified tribe. If no cell of the specified tribe is found, return null.
        /// </summary>
        /// <param name="targetTribeName"></param>
        /// <returns></returns>
        public static List<Cell> DetectTribe(string targetTribeName)
        {
            var cells = FindObjectsOfType(typeof(Cell)) as Cell[];
            var detectedCells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (cell.Tribe.Name == targetTribeName)
                {
                    detectedCells.Add(cell);
                }
            }
            return detectedCells;
        }
        /// <summary>
        /// 指定ランクの種族セルをリストにして返す
        /// </summary>
        /// <param name="rank"></param>
        /// <returns>指定ランクのセル</returns>
        public static List<Cell> DetectTribe(Rank rank)
        {
            var cells = FindObjectsOfType(typeof(Cell)) as Cell[];
            var detectedCells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (cell.Tribe.Rank == rank)
                {
                    detectedCells.Add(cell);
                }
            }
            return detectedCells;
        }
        /// <summary>
        /// 自種族と敵種族で,自種族が上位の場合true,下位の場合false.
        /// </summary>
        /// <param name="myTribe">自種族</param>
        /// <param name="enemyTribe">敵種族</param>
        /// <returns>相手より生態系ランキングが上かどうか</returns>
        public static bool IsGreaterRank(string myTribe, string enemyTribe)
        {
            return true; // check
        }
        public void SyncRanking()
        {
            Tribes.OrderBy(tribe => tribe.Rank);
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        }
        /// <summary>
        /// Called when this species is extinct.
        /// </summary>
        public static void Extinction(TribeStatus tribe)
        {
            Debug.Log($"{tribe.Name} is extinct");
            if (tribe.Name == m_instance.playersTribe.Name)
            {
                //m_instance.stageManager.GameOver();
                // TODO: 自族だった場合はゲームオーバー処理
            }
        }
        public static void GenerateAtom()
        {
            var spawnPoint = Random.onUnitSphere * m_instance.mapSquare;
            spawnPoint.y = 5f;
            m_instance.generateAtom(spawnPoint);
        }
        void generateAtom(Vector3 spawnPoint)
        {
            var atomObj = Instantiate(atom, spawnPoint, Quaternion.identity) as GameObject;
            atomObj.transform.localScale *= 2;
            AtomsOnStage.Add(atomObj.GetComponent<Atom>());
        }
        /// <summary>
        /// 対象のセルと同じ種族のセルを新しく生成する
        /// </summary>
        /// <param name="masterCell"></param>
        /// <returns></returns>
        public static Cell GenerateTribe(Cell masterCell)
        {
            if (masterCell.MyLeaderTransform != null)
            {
                return m_instance.GenerateTribe(masterCell.Tribe, masterCell.MyLeaderTransform.position);
            }
            return null;
        }
        /// <summary>
        /// 対象の種族を指定した座標に生成する.引数に応じて役職をつける
        /// </summary>
        /// <param name="tribe"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        Cell GenerateTribe(TribeStatus tribe, Vector3 spawnPosition, CellFlags flag = CellFlags.None)
        {
            var go = new GameObject(tribe.Name);
            go.layer = LayerMask.NameToLayer("Cell");
            go.tag = "Cell";
            spawnPosition += Random.onUnitSphere * sphereScalar;
            spawnPosition.y = spawnHeight;
            go.transform.position = spawnPosition;
            var gizmo = new GameObject("GizmoObj", typeof(GizmoObject));
            gizmo.transform.parent = go.transform;
            gizmo.transform.localPosition = Vector3.zero;
            var cell = go.AddComponent<Cell>();
            //cell.Tribe = new TribeStatus(tribe);
            var collider = go.AddComponent<MeshCollider>();
            collider.convex = true;
            cell.ASource = go.AddComponent<AudioSource>();
            cell.ASource.volume = .1f;
            //cell.transform.localScale *= 3;

            switch (flag)
            {
                case CellFlags.None:
                    break;
                case CellFlags.IsGrounded:
                    break;
                case CellFlags.IsLeader:
                    cell.IsLeader = true;
                    break;
                case CellFlags.IsPlayer:
                    cell.IsPlayer = true;
                    cell.IsLeader = true;
                    break;
                case CellFlags.IsBoss:
                    cell.IsBoss = true;
                    break;
                default:
                    break;
            }
            return cell;
        }

        /// <summary>
        /// 対象の種族を指定した座標に生成する.引数に応じて役職をつける
        /// </summary>
        /// <param name="tribe"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        Cell GenerateTribe(string tribe, Vector3 spawnPosition, CellFlags flag = CellFlags.None)
        {
            var prefab = Resources.Load<GameObject>("Cells/Cell");
            var go = Instantiate(prefab, spawnPosition, Quaternion.identity);
            var cell = go.GetComponent<Cell>();
            cell.Tribe = Tribes.First(t => t.Name.Equals(tribe));

            ReplaceMesh(go, cell.Tribe);

            cell.ASource = go.AddComponent<AudioSource>();
            cell.ASource.volume = .1f;
            //cell.transform.localScale *= 3;

            switch (flag)
            {
                case CellFlags.None:
                    break;
                case CellFlags.IsGrounded:
                    break;
                case CellFlags.IsLeader:
                    cell.IsLeader = true;
                    break;
                case CellFlags.IsPlayer:
                    cell.IsPlayer = true;
                    cell.IsLeader = true;
                    break;
                case CellFlags.IsBoss:
                    cell.IsBoss = true;
                    break;
                default:
                    break;
            }
            return cell;
        }

        /// <summary>
        /// 種族に応じたメッシュ差し替え
        /// </summary>
        /// <param name="go"><see cref="GameObject"/> of <see cref="Cell"/></param>
        /// <param name="tribe">Specified tribe of <see cref="Cell"/></param>
         void ReplaceMesh(GameObject go, TribeStatus tribe)
        {
            var renderer = go.AddComponent<SkinnedMeshRenderer>();
            var collider = go.AddComponent<MeshCollider>();
            renderer.sharedMesh = tribe.Mesh;
            collider.sharedMesh = tribe.Mesh;
            collider.convex = true;
        }

        /// <summary>
        /// 対象ランクの全ての種族セルをAmount分生成する
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="generateAmount"></param>
        public static void GenerateTribe(Rank rank, int generateAmount)
        {
            List<TribeStatus> tribes = m_instance.MasterData.Tribes.FindAll(tribe => tribe.Rank == rank);
            Tribes = tribes;
            tribes = tribes.OrderBy(tribe => Guid.NewGuid()).ToList();

            var points = m_instance.spawnPointsParent.GetComponentsInChildren<Transform>().ToList();

            foreach (var tribe in tribes)
            {
                var cells = DetectTribe(tribe.Name);
                var isExistLeader = cells.Any(c => c.IsLeader);
                var randomScalar = Random.Range(-75, 75);
                var spawnTransform = points[Random.Range(0, points.Count() - 1)];
                points.Remove(spawnTransform);
                for (int count = 0; count < generateAmount; ++count)
                {
                    var pos = spawnTransform.position + Random.onUnitSphere * count;
                    if (!PlayerCell)
                    {
                        m_instance.GenerateTribe(tribe, pos, CellFlags.IsPlayer);
                    }
                    else if (!isExistLeader && count == 0)
                    {
                        m_instance.GenerateTribe(tribe, pos, CellFlags.IsLeader);
                    }
                    else
                    {
                        m_instance.GenerateTribe(tribe, pos, CellFlags.None);
                    }
                }
            }
            m_instance.GenerateBoss(tribes[Random.Range(0, tribes.Count() - 1)], points[Random.Range(0, points.Count() - 1)].position);
        }
        public void GenerateBoss(TribeStatus tribeData, Vector3 spawnPosition)
        {
            var boss = GenerateTribe(tribeData, spawnPosition, CellFlags.IsBoss);
            boss.transform.localScale *= 2;
            boss.Tribe.Basic.Life = 30;
            boss.gameObject.name = "Boss";
        }
        /// <summary>
        /// Detect Player cell. if not found then return null.
        /// </summary>
        /// <returns></returns>
        Cell DetectPlayerCell()
        {
            var playerCell = CellsOntheStage.SingleOrDefault(cell => cell.IsPlayer);
            if (!playerCell)
            {
                var tribalCells = DetectTribe(playersTribe?.Name);
                if (tribalCells.Count == 0)
                {
                    //TODO: Implement process game over 
                    return null;
                }
            }
            playersTribe = playerCell?.Tribe;
            return playerCell;
        }
        #endregion
        #region Callbacks
        private void Awake()
        {
            //DetectNewTribes();
            atom = Resources.Load("Atom") as GameObject;
        }
        private void Start()
        {
            DetectNewTribes();
        }
        float elapsedTime;

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > atomGenerationInterval && AtomsOnStage.Count < atomsLimit)
            {
                elapsedTime = 0;
                GenerateAtom();
            }
        }
        #endregion
        #region Enumerables
        public enum Rank
        {
            S,
            AAA,
            AA,
            A,
            B,
            C,
            D,
            E,
            F,
        }
        #endregion
    }
}