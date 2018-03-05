using System;
using System.Collections;
using UnityEngine;
using ZFramework.Resource;

namespace ZFramework.Runtime
{
    /// <summary>
    /// 资源组件。
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceComponent : ZFrameworkComponent
    {
        private IResourceManager m_ResourceManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_ResourceManager = ZFrameworkEntry.GetModule<IResourceManager>();
            if (m_ResourceManager == null)
            {
                Log.Fatal("Resource manager is invalid.");
                return;
            }
        }

        private void Start()
        {
        }

        /// <summary>
        /// 设置资源服务器地址
        /// </summary>
        /// <param name="url"></param>
        public void SetBundleBaseURL(string url)
        {
            m_ResourceManager.SetBundleBaseURL(url);
        }

        /// <summary>
        /// 预下载初始AssetBundle资源
        /// </summary>
        /// <returns></returns>
        public IEnumerator PreDownloadInitBundle()
        { 
            IEnumerator e = m_ResourceManager.Initialize();
            while (e.MoveNext()) { yield return null; }
        }


        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <returns></returns>
        public float GetDownloadProgress()
        {
            return m_ResourceManager.GetDownloadProgress();
        }


        public void GetLoadedBundle(string assetBundleName, Action<AssetBundle> action)
        {
            m_ResourceManager.GetLoadedBundle(assetBundleName, action);
        }

        /// <summary>
        /// 获取各种类型的Asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        public void LoadAsset<T>(string assetName, Action<T> action) where T : UnityEngine.Object
        {
            m_ResourceManager.LoadAsset<T>(assetName, action);
        }

        /// <summary>
        /// 卸载bundle资源。
        /// </summary> 
        public void UnloadBundle(string bundle)
        {
            m_ResourceManager.UnloadBundle(bundle);
        }


        public void LoadDataTable(string filePath, Action<object> action)
        {
            object bindata = null;
            try
            {
                bindata = Resources.Load(filePath, typeof(TextAsset));
            }
            catch (System.Exception e)
            {
                throw e;
            }

            action.Invoke(bindata);
        }

        /// <summary>
        /// 加载Sprite   ———————————— 后续改成AB加载
        /// </summary>
        public Sprite LoadSprite(string filePath)
        {
            Sprite sp = null;
            try
            {
                sp = Resources.Load(filePath, typeof(Sprite)) as Sprite;
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return sp;
        }

        /// <summary>
        /// 加载Texture   ———————————— 后续改成AB加载
        /// </summary>
        public Texture LoadTexture(string filePath)
        {
            Texture tex = null;
            try
            {
                tex = Resources.Load(filePath, typeof(Texture)) as Texture;
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return tex;
        }

        /// <summary>
        /// 加载Texture   ———————————— 后续改成AB加载
        /// </summary>
        public RenderTexture LoadRenderTexture(string filePath)
        {
            RenderTexture tex = null;
            try
            {
                tex = Resources.Load(filePath, typeof(RenderTexture)) as RenderTexture;
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return tex;
        }

        private void OnDestroy()
        {
            //清理资源
        }
    }
}