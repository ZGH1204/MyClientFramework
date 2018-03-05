using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ZFramework.Resource
{
    /// <summary>
    /// 资源管理器。
    /// </summary>
    internal sealed partial class ResourceManager : ZFrameworkModule, IResourceManager
    {
        public ResourceManager()
        {
        }

        /// <summary>
        /// 设置资源服务器地址
        /// </summary>
        /// <param name="url"></param>
        public void SetBundleBaseURL(string url)
        {
            BundleManager.SetBundleBaseURL(url);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public IEnumerator Initialize()
        {
            IEnumerator e = BundleManager.Initialize();
            while (e.MoveNext()) { yield return null; }

            e = BundleManager.PreDownloadInitBundle();
            while (e.MoveNext()) { yield return null; }
        }
        
        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <returns></returns>
        public float GetDownloadProgress()
        {
            return BundleManager.GetDownloadProgress();
        }

        /// <summary>
        /// 获取AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="action"></param>
        public void GetLoadedBundle(string assetBundleName, Action<AssetBundle> action)
        {
            string error = string.Empty;
            AssetBundle ab = BundleManager.GetLoadedBundle(assetBundleName, out error);
            if (ab == null)
            {
                Log.Info(error);
                Runtime.CoroutineHelper.Instance.StartCoroutine(BundleManager.StartDownloadBundle(assetBundleName, action));
            }
            else
            {
                action.Invoke(ab);
            }
        }
         
        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        internal override void Shutdown()
        {
        }


        /// <summary>
        /// 获取各种类型的Asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        public void LoadAsset<T>(string assetName, Action<T> action) where T : UnityEngine.Object
        {
            T obj = default(T);
            string bundleName = BundleManager.GetBundleNameByAsset(assetName);
#if UNITY_EDITOR
            if (BundleManager.usedAssetBundle)
            {
                // 编辑器需要标记好资产，才能使用该API
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Log.Error("There is no asset with name \"" + assetName + "\" in " + assetName);
                }
                else
                {
                    UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
                    obj = AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
                    action.Invoke(obj);
                }
            }
            else
#endif
            { 
                GetLoadedBundle(bundleName, (ab)=> {
                    if (ab == null)
                    {
                        return;
                    }

                    T asset = ab.LoadAsset<T>(assetName);
                    action.Invoke(asset);
                });
              
            }
        }


        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset(string assetName, LoadAssetCallbackEvent loadAssetCallbacks)
        {
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, LoadAssetCallbackEvent loadAssetCallbacks, object userData)
        {
            var loginA = Resources.Load(assetName, typeof(GameObject));
            if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
            {
                loadAssetCallbacks.LoadAssetSuccessCallback.Invoke(assetName, loginA, GetDownloadProgress(), userData);
            }
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadBundle(string bundle)
        {
            BundleManager.UnloadAssetBundle(bundle);
        }
    }
}