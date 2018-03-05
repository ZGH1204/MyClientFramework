using System;
using System.Collections;
using UnityEngine;

namespace ZFramework.Resource
{
    public interface IResourceManager
    {
        /// <summary>
        /// 设置AssetBundle资源服务器地址
        /// </summary>
        /// <param name="url"></param>
        void SetBundleBaseURL(string url);

        /// <summary>
        /// 预下载初始AssetBundle资源
        /// </summary>
        /// <returns></returns>
        IEnumerator Initialize();

        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <returns></returns>
        float GetDownloadProgress();

        /// <summary>
        /// 获取AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="action"></param>
        void GetLoadedBundle(string assetBundleName, Action<AssetBundle> action);

        /// <summary>
        /// 获取各种类型的Asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="action"></param>
        void LoadAsset<T>(string assetName, Action<T> action) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, LoadAssetCallbackEvent loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, LoadAssetCallbackEvent loadAssetCallbacks, object userData);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadBundle(string bundle);
    }
}