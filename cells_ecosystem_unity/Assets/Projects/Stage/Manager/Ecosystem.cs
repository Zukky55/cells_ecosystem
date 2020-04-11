using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using System;

namespace CellsEcosystem.tmp
{
    public class Ecosystem : SingletonMonoBehaviour<Ecosystem>
    {
        [Serializable]
        class Param
        {
            [Serializable]
            public class AtomParam
            {
                public int resourcesAmount;
            }

            [Serializable]
            public class FieldObjectPattern
            {
                /// <summary>
                /// 種族をまとめたルートオブジェクト
                /// </summary>
                public GameObject[] tribesPrefab;

            }


            public AtomParam atomParam = new AtomParam();
            public FieldObjectPattern fieldPattern = new FieldObjectPattern();
        }

        /// <summary>
        /// セルのマスターデータ
        /// </summary>
        [SerializeField]
        CellsData masterData;

        /// <summary>
        /// エコシステムのパラメータ
        /// </summary>
        [SerializeField]
        Param param;

        ResourceManager<Atom> atomResource;

        private void OnEnable()
        {
            Game.SubscribeStateEvent(GameStateMachine<Game.State>.When.Enter, OnInitialize);
        }

        void OnInitialize(Game.State state)
        {
            if (state != Game.State.InitGame)
            {
                return;
            }

            Initialize();
        }

        void Initialize()
        {
            atomResource = new ResourceManager<Atom>(param.atomParam.resourcesAmount);


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
            cell.Tribe = masterData.Tribes.First(t => t.Name.Equals(tribe));

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
    }



    ///// <summary>
    ///// Parameters
    ///// </summary>
    //public partial class Ecosystem
    //{
    //    public class Param
    //    {

    //    }
    //}

}