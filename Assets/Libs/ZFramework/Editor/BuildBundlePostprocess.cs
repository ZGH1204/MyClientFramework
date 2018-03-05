using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZFramework.Editor
{
    public class BuildBundlePostprocess
    {
        public static void DoPostprocess(AssetBundleManifest buildManifest, AssetBundleBrowser.AssetBundleDataSource.ABBuildInfo info)
        {
            Resource.BundleList bundleList = new ZFramework.Resource.BundleList();
            bundleList.bundles = new List<Resource.BundleList.URLVersionPair>();

            foreach (var assetBundleName in buildManifest.GetAllAssetBundles())
            {
                bundleList.bundles.Add(new Resource.BundleList.URLVersionPair()
                {
                    bundleName = assetBundleName,
                    bundleSize = new FileInfo(info.outputDirectory + "/" + assetBundleName).Length,
                    hashCode = buildManifest.GetAssetBundleHash(assetBundleName).GetHashCode(),
                    preDownload = true
                });
            }

            //保存BundleList
            System.Type jsonHelperType = Utility.Assembly.GetTypeWithinLoadedAssemblies("ZFramework.Runtime.JsonHelper");
            ZFramework.Utility.Json.IJsonHelper jsonHelper = (ZFramework.Utility.Json.IJsonHelper)Activator.CreateInstance(jsonHelperType);
            ZFramework.Utility.Json.SetJsonHelper(jsonHelper);
            string json_bundleList = ZFramework.Utility.Json.ToJson(bundleList);
            StreamWriter fileWriter = new StreamWriter(info.outputDirectory + "/BundleList.json");
            fileWriter.WriteLine(json_bundleList);
            fileWriter.Close();
            fileWriter.Dispose();
        }
    }
}