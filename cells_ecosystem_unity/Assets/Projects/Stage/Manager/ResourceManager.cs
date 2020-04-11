using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CellsEcosystem
{
    public interface IResource
    {
        bool IsUsing { get; }
        bool IsInitialized { get; }

        void Activate();
        void Deactivate();
    }

    /// <summary>
    /// 生成数が多いリソースを管理する
    /// </summary>
    public class ResourceManager<TResource> where TResource : IResource
    {
        //#region Singleton
        //static ResourceManager<TResource> instance;
        //public static ResourceManager<TResource> Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new ResourceManager<TResource>();
        //        }
        //        return instance;
        //    }
        //}
        //#endregion

        TResource[] resources;
        bool isInitialized;


        public bool IsInitialized => isInitialized;

        public ResourceManager(int amount)
        {
            resources = new TResource[amount];
            Game.SubscribeStateEvent(GameStateMachine<Game.State>.When.Enter, OnInitializeEnter);
        }
        ~ResourceManager()
        {
            if (!Game.IsEmpty)
            {
                Game.DisposeStateEvent(GameStateMachine<Game.State>.When.Enter, OnInitializeEnter);
            }
        }

        private void OnInitializeEnter(Game.State state)
        {
            if (!state.Equals(Game.State.InitGame)) return;
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < resources.Length; i++)
            {
                var prefab = Resources.Load<GameObject>(typeof(TResource).ToString());
                var go = GameObject.Instantiate(prefab, new Vector3(100f, 100f, -100f), Quaternion.identity);
                var resource = go.GetComponent<TResource>();
                resource.Deactivate();
                resources[i] = resource;
            }
        }

        public TResource GetResource(bool isUsing)
        {
            if (!isInitialized) throw new Exception("初期化されてへんで");

            var res = resources.FirstOrDefault(a => a.IsUsing.Equals(isUsing));
            if (res == null)
            {
                throw new NullReferenceException($"リソースに[isUsing={isUsing}]の該当なし");
            }
            return res;
        }
    }
}