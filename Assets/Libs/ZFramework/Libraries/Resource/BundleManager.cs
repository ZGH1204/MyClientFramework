using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFramework.Resource
{
    /// <summary>
    /// Bundle对比辅助类
    /// </summary>
    public class BundleList
    {
        public class URLVersionPair
        {
            public string bundleName;
            public long bundleSize;
            public int hashCode;
            public bool preDownload;
        }

        public List<URLVersionPair> bundles;

        public URLVersionPair SearchURL(string str)
        {
            return bundles.Find(x => x.bundleName.Equals(str, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// 加载Bundle辅助类
    /// </summary>
    public class LoadedAssetBundle
    {
        public AssetBundle assetBundle;
        public int referencedCount;

        internal event Action unload;

        public LoadedAssetBundle(AssetBundle bundle)
        {
            this.assetBundle = bundle;
            referencedCount = 1; // 引用N次，执行N+1次卸载才能卸载；避免反复加载卸载！
        }

        internal void OnUnload()
        {
            assetBundle.Unload(false);
            if (unload != null)
            {
                unload();
            }
        }
    }

    public class BundleManager
    {
        public static bool usedAssetBundle = true;
        public static string cacheAssetPath = Utility.Path.GetPersistentDataPath() + "/Bundle/";
        public static string fileBaseURL = string.Empty;
        public static string bundleBaseURL = string.Empty;
        private static int m_MultiDownloadNum = 5;
        private static Queue<string> m_ActiveVariantsQueue = new Queue<string>();
        private static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        private static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        private static BundleList m_ServerBundleList;
        private static BundleList m_LocalBundleList;
        private static AssetBundleManifest m_AssetBundleManifest = null;

        private static WWW m_Downloader = null;
        private static Queue<BundleList.URLVersionPair> m_MultiDownloadQueue;
        private static WWW[] m_MultiDownloader = new WWW[m_MultiDownloadNum];
        private static bool[] m_MultiBusyDownloading = new bool[m_MultiDownloadNum];
        private static bool m_IsMultiDownloading = false;

        private static int m_TotalPreDownloadNum = 1;
        private static int m_CurrentPreDownloadNum = 0;

        /// <summary>
        /// Variants which is used to define the active variants.
        /// </summary>
        public static Queue<string> ActiveVariantsQueue
        {
            get { return m_ActiveVariantsQueue; }
            set { m_ActiveVariantsQueue = value; }
        }

        /// <summary>
        /// 是否正在下载
        /// </summary>
        /// <returns></returns>
        public static bool DoneLoading()
        {
            return !m_IsMultiDownloading;
        }

        /// <summary>
        /// 设置资源服务器地址
        /// </summary>
        /// <param name="url"></param>
        public static void SetBundleBaseURL(string url)
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                return;
#endif
            fileBaseURL = url;
#if UNITY_IOS
			bundleBaseURL =  fileBaseURL + "update/AB_Output/ios/";
#elif UNITY_ANDROID
            bundleBaseURL = fileBaseURL + "update/AB_Output/";
#endif
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static IEnumerator Initialize()
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                yield break;
#endif
            if (bundleBaseURL.Equals(string.Empty))
            {
                Log.Error("***********      未设置资源服务器地址!      ***********");
            }

            m_MultiDownloadQueue = new Queue<BundleList.URLVersionPair>();
            for (int x = 0; x < m_MultiDownloadNum; ++x)
            {
                m_MultiDownloader[x] = null;
                m_MultiBusyDownloading[x] = false;
            }

            // 创建缓存目录
            if (!System.IO.Directory.Exists(cacheAssetPath))
            {
                System.IO.Directory.CreateDirectory(cacheAssetPath);
            }
            if (System.IO.File.Exists(cacheAssetPath + "BundleList.json"))
            {
                string encodedString = System.IO.File.ReadAllText(cacheAssetPath + "BundleList.json");
                //string bundleListJson = Utility.Encryption.Decrypt(encodedString);
                m_LocalBundleList = Utility.Json.ToObject<BundleList>(encodedString);
            }
            else
            {
                m_LocalBundleList = new BundleList();
                m_LocalBundleList.bundles = new List<BundleList.URLVersionPair>();
                SaveLocalBundleList();
            }

            // 下载ServerBundleList
            while (true)
            {
                m_Downloader = new WWW(bundleBaseURL + "BundleList.json");
                yield return m_Downloader;
                if (m_Downloader.error == null)
                {
                    m_ServerBundleList = Utility.Json.ToObject<BundleList>(m_Downloader.text);
                    break;
                }
                else
                {
                    Log.Error("Download serverBundleList Failed !");
                }
            }

            //清除本地不再使用的Bundle
            ClearUnUsedBundle();

            // 下载AssetBundleManifest
            while (true)
            {
                m_Downloader = new WWW(bundleBaseURL + "BundleManifest");
                yield return m_Downloader;
                if (m_Downloader.error == null)
                {
                    m_AssetBundleManifest = m_Downloader.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    break;
                }
                else
                {
                    Log.Error("Download serverBundleList Failed !");
                }
            }
        }

        /// <summary>
        /// 获取已经加载好的AssetBundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static AssetBundle GetLoadedBundle(string assetBundleName, out string error)
        {
            RemapVariantName(ref assetBundleName);
            LoadedAssetBundle bundle = null; //是否已经加载
            if (!m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
            {
                error = "AssetBundle: " + assetBundleName + " 未加载！";
                return null;
            }

            string[] dependencies = null;
            if (m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                foreach (var dependency in dependencies) // 确保依赖都被加载了
                {
                    LoadedAssetBundle dependentBundle;
                    m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                    if (dependentBundle == null)
                    {
                        error = "AssetBundle的依赖: " + dependency + " 未加载！";
                        return null;
                    }
                }
            }

            error = "返回AssetBundle: " + assetBundleName;
            bundle.referencedCount++;
            return bundle.assetBundle;
        }

        /// <summary>
        /// 通过资产名字获取对应的AssetBundle
        /// </summary>
        public static string GetBundleNameByAsset(string assetName)
        {
            return "sprites_run";
        }

        /// <summary>
        /// Bundle是否已经下载
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public static bool IsBundleDownloaded(string assetBundleName)
        {
            RemapVariantName(ref assetBundleName);
            return m_LoadedAssetBundles.ContainsKey(assetBundleName);
        }

        /// <summary>
        /// 预下载初始AssetBundle资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerator PreDownloadInitBundle()
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                yield break;
#endif
            if (m_ServerBundleList != null)
            {
                foreach (BundleList.URLVersionPair aBundle in m_ServerBundleList.bundles)
                {
                    if (aBundle.preDownload == true)
                    {
                        AddMultiDownloadBundleQueue(aBundle.bundleName);
                    }
                }
            }
            else
            {
                Log.Error("ServerBundleList is null !");
            }
            IEnumerator e = StartMultiDownloadBundle();
            while (e.MoveNext()) { yield return null; }

            Resources.UnloadUnusedAssets();
        }

        #region 下载

        /// <summary>
        /// 添加AssetBundle到下载队列
        /// </summary>
        /// <param name="bundleName"></param>
        public static void AddMultiDownloadBundleQueue(string bundleName)
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                return;
#endif
            if (string.IsNullOrEmpty(bundleName))
            {
                return;
            }
            RemapVariantName(ref bundleName);
            foreach (BundleList.URLVersionPair aBundle in m_ServerBundleList.bundles)
            {
                if (aBundle.bundleName.CompareTo(bundleName) == 0)
                {
                    if (!(m_MultiDownloadQueue.Contains(aBundle)))
                    {
                        m_MultiDownloadQueue.Enqueue(aBundle);
                        AddDownloadBundleDependenciesQueue(bundleName);
                    }
                    return;
                }
            }

            Log.Warning("No asset bundle in the *List* : " + bundleName);
        }

        /// <summary>
        /// 将bundle的依赖加入下载队列
        /// </summary>
        /// <param name="bundleName"></param>
        private static void AddDownloadBundleDependenciesQueue(string bundleName)
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                return;
#endif
            if (m_AssetBundleManifest == null)
            {
                Log.Error("Please initialize AssetBundleManifest !");
                return;
            }

            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(bundleName);
            if (dependencies.Length != 0)
            {
                m_Dependencies.Add(bundleName, dependencies);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    AddMultiDownloadBundleQueue(dependencies[i]);
                }
            }
        }

        /// <summary>
        /// 利用协成开始下载AssetBundle
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartDownloadBundle(string bundleName, Action<AssetBundle> back)
        {
            while (m_IsMultiDownloading)
            {
                yield return null;
            }

            AddMultiDownloadBundleQueue(bundleName);
            IEnumerator e = StartMultiDownloadBundle();
            while (e.MoveNext()) { yield return null; }

            string error = string.Empty;
            AssetBundle ab = BundleManager.GetLoadedBundle(bundleName, out error);
            if (ab == null)
            {
                Log.Info(error);
            }
            back.Invoke(ab);
        }

        /// <summary>
        /// 利用协成开始多个下载AssetBundle
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartMultiDownloadBundle()
        {
            m_IsMultiDownloading = true;

            bool AllComplete = true;

            m_TotalPreDownloadNum = m_MultiDownloadQueue.Count;
            m_CurrentPreDownloadNum = 0;

            while (true)
            {
                AllComplete = true;
                for (int x = 0; x < m_MultiDownloadNum; ++x)
                {
                    if (m_MultiBusyDownloading[x] == false)
                    {
                        if (m_MultiDownloadQueue.Count > 0)
                        {
                            Runtime.CoroutineHelper.Instance.StartCoroutine(DownloadBundleComplete(x, m_MultiDownloadQueue.Dequeue()));
                            AllComplete = false;
                        }
                    }
                    else
                    {
                        AllComplete = false;
                    }
                }

                if (AllComplete)
                {
                    break; // 跳出while循环
                }

                yield return null; // 暂停协同程序，下一帧再继续往下执行,防止一帧就卡死
            }

            m_IsMultiDownloading = false;
            m_Downloader = null;
        }

        /// <summary>
        /// 下载AssetBundle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aBundle"></param>
        /// <returns></returns>
        private static IEnumerator DownloadBundleComplete(int id, BundleList.URLVersionPair aBundle)
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                yield break;
#endif
            if (CheckLocalBundle(aBundle.bundleName))
            {
                LoadLocalBundle(aBundle.bundleName);
                yield break;
            }

            Log.Info("开始下载AssetBundle： " + aBundle.bundleName);
            m_MultiBusyDownloading[id] = true;
            while (true)
            {
                m_MultiDownloader[id] = new WWW(bundleBaseURL + aBundle.bundleName);
                yield return m_MultiDownloader[id];
                if (m_MultiDownloader[id].error == null)
                {
                    try
                    {
                        System.IO.File.WriteAllBytes(cacheAssetPath + aBundle.bundleName, m_MultiDownloader[id].bytes);
                        LoadLocalBundle(aBundle.bundleName, m_MultiDownloader[id].bytes);
                        SaveLocalBundleList(aBundle.bundleName);
                    }
                    catch (System.Exception e)
                    {
                        Log.Info(e.Message);
                    }
                    break;
                }
                else
                {
                    Log.Error(" 下载 AssetBundle 出错 " + aBundle.bundleName);
                }
            }

            ++m_CurrentPreDownloadNum;
            m_MultiBusyDownloading[id] = false;
        }

        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <returns></returns>
        public static float GetDownloadProgress()
        {
            float ratio = 0;
            if (m_TotalPreDownloadNum == 0)
            {
                ratio = 1;
            }
            else
            {
                ratio = (float)m_CurrentPreDownloadNum / (float)m_TotalPreDownloadNum;
            }
            if (m_TotalPreDownloadNum <= 1 && m_Downloader != null)
            {
                ratio = m_Downloader.progress;
            }

            if (float.IsNaN(ratio) || float.IsInfinity(ratio))
            {
                ratio = 1.0f;
            }
            ratio = Mathf.Clamp01(ratio);
            return ratio;
        }

        #endregion 下载

        #region 卸载

        /// <summary>
        /// Unloads assetbundle and its dependencies.
        /// </summary>
        public static void UnloadAssetBundle(string assetBundleName)
        {
#if UNITY_EDITOR
            if (usedAssetBundle)
                return;
#endif
            RemapVariantName(ref assetBundleName);
            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);
        }

        private static void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                return;
            }

            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }

            m_Dependencies.Remove(assetBundleName);
        }

        private static void UnloadAssetBundleInternal(string assetBundleName)
        {
            LoadedAssetBundle bundle = null;
            if (!m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
            {
                Log.Info("没有找到要卸载的Bundle: " + assetBundleName);
                return;
            }

            if (--bundle.referencedCount == 0)
            {
                bundle.OnUnload();
                m_LoadedAssetBundles.Remove(assetBundleName);

                Log.Info("Bundle: " + assetBundleName + " 卸载成功 !");
            }
        }

        #endregion 卸载

        /// <summary>
        /// 重新映射BundleName
        /// </summary>
        private static void RemapVariantName(ref string assetBundleName)
        {
            if (m_ActiveVariantsQueue.Count == 0)
            {
                return;
            }

            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();
            string baseName = assetBundleName.Split('.')[0];
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != baseName)
                {
                    continue;
                }

                if (m_ActiveVariantsQueue.Contains(curSplit[1]))
                {
                    assetBundleName = bundlesWithVariant[i];
                    break;
                }
            }
        }

        /// <summary>
        /// 检查内存中是否存在AssetBundle或者是否有更新
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private static bool CheckLocalBundle(string bundleName)
        {
            if (!System.IO.File.Exists(cacheAssetPath + bundleName))
            {
                Log.Info("本地不存在AssetBundle： " + bundleName);
                return false;
            }

            BundleList.URLVersionPair serverInfo = m_ServerBundleList.SearchURL(bundleName);
            BundleList.URLVersionPair localInfo = m_LocalBundleList.SearchURL(bundleName);

            if (serverInfo == null || localInfo == null)
            {
                Log.Info("本地AssetBundle： " + bundleName + " 不存在!");
                return false;
            }

            if (serverInfo.hashCode != localInfo.hashCode)
            {
                Log.Info("本地AssetBundle： " + bundleName + " 有更新!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从内存中加载AssetBundle到缓存中
        /// </summary>
        /// <param name="bundleName"></param>
        private static void LoadLocalBundle(string bundleName)
        {
            if (m_LoadedAssetBundles.ContainsKey(bundleName))
            {
                return;
            }

            byte[] bundleByte = System.IO.File.ReadAllBytes(cacheAssetPath + bundleName);
            AssetBundle loadedAssetBundle = AssetBundle.LoadFromMemory(bundleByte);
            if (!bundleName.StartsWith("Images_"))
            {
                ReSetBundleShaders(loadedAssetBundle);
            }
            m_LoadedAssetBundles.Add(bundleName, new LoadedAssetBundle(loadedAssetBundle));
        }

        /// <summary>
        /// 从内存中加载AssetBundle到缓存中
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="bundleByte"></param>
        private static void LoadLocalBundle(string bundleName, byte[] bundleByte)
        {
            if (m_LoadedAssetBundles.ContainsKey(bundleName))
            {
                return;
            }

            AssetBundle loadedAssetBundle = AssetBundle.LoadFromMemory(bundleByte);
            if (!bundleName.StartsWith("Images_"))
            {
                ReSetBundleShaders(loadedAssetBundle);
            }
            m_LoadedAssetBundles.Add(bundleName, new LoadedAssetBundle(loadedAssetBundle));
        }

        /// <summary>
        /// 重新索引Bundle的Shader
        /// </summary>
        /// <param name="loadedAssetBundle"></param>
        private static void ReSetBundleShaders(AssetBundle loadedAssetBundle)
        {
            if (loadedAssetBundle == null)
            {
                Log.Warning("ReSetBundleShaders is null！ ");
                return;
            }
            if (loadedAssetBundle.isStreamedSceneAssetBundle)
            {
                return;
            }

            var materials = loadedAssetBundle.LoadAllAssets(typeof(Material));
            foreach (Material m in materials)
            {
                var shaderName = m.shader.name;
                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    m.shader = newShader;
                }
                else
                {
                    Log.Warning("unable to refresh shader: " + shaderName + " in material " + m.name);
                }
            }
            // ... ...
        }

        /// <summary>
        /// 保存本地的BundleList
        /// </summary>
        private static void SaveLocalBundleList(string bundleName)
        {
            BundleList.URLVersionPair serverInfo = m_ServerBundleList.SearchURL(bundleName);
            BundleList.URLVersionPair localInfo = m_LocalBundleList.SearchURL(bundleName);
            if (serverInfo != null)
            {
                if (localInfo != null)
                {
                    m_LocalBundleList.bundles.Remove(localInfo);
                }
                m_LocalBundleList.bundles.Add(serverInfo);
            }

            SaveLocalBundleList();
        }

        /// <summary>
        /// 保存本地的BundleList
        /// </summary>
        private static void SaveLocalBundleList()
        {
            string bundleListJson = Utility.Json.ToJson(m_LocalBundleList);
            //string encodedstring = Utility.Encryption.EncryptString(bundleListJson);

            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(cacheAssetPath + "BundleList.json", false);
            fileWriter.WriteLine(bundleListJson);
            fileWriter.Close();
            fileWriter.Dispose();
        }

        /// <summary>
        /// 清除本地不再使用的Bundle
        /// </summary>
        private static void ClearUnUsedBundle()
        {
            for (int i = 0; i < m_LocalBundleList.bundles.Count; i++)
            {
                if (m_ServerBundleList.SearchURL(m_LocalBundleList.bundles[i].bundleName) == null)
                {
                    if (System.IO.File.Exists(cacheAssetPath + m_LocalBundleList.bundles[i].bundleName))
                    {
                        System.IO.File.Delete(cacheAssetPath + m_LocalBundleList.bundles[i].bundleName);
                        m_LocalBundleList.bundles.RemoveAt(i);
                        i--;
                    }
                }
            }

            SaveLocalBundleList();
        }

        /// <summary>
        /// 清除所有的本地的Bundle
        /// </summary>
        public static void ClearAllBundle()
        {
            for (int i = 0; i < m_LocalBundleList.bundles.Count; i++)
            {
                if (System.IO.File.Exists(cacheAssetPath + m_LocalBundleList.bundles[i].bundleName))
                {
                    System.IO.File.Delete(cacheAssetPath + m_LocalBundleList.bundles[i].bundleName);
                    m_LocalBundleList.bundles.RemoveAt(i);
                    i--;
                }
            }

            SaveLocalBundleList();
        }
    }
}