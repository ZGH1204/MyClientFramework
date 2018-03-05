﻿using System;
using System.IO;
using UnityEngine;

namespace ZFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 路径相关的实用函数。
        /// </summary>
        public static class Path
        {
            /// <summary>
            /// 获取各个平台streamingAssetsPath文件夹的地址
            /// </summary>
            /// <returns>streamingAssetsPath文件夹的地址</returns>
            public static string GetStreamingAssetsPath()
            {
                string streamingAssetPath = "";

#if UNITY_EDITOR
                streamingAssetPath = "file:///" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
	            streamingAssetPath = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS
	            streamingAssetPath = "file:///" + Application.dataPath + "/Raw/";
#else
	            streamingAssetPath = "file:///" + Application.dataPath + "/StreamingAssets/";
#endif

                return streamingAssetPath;
            }

            /// <summary>
            /// 获取各个平台persistentDataPath文件夹的地址
            /// </summary>
            /// <returns>persistentDataPath文件夹的地址</returns>
            public static string GetPersistentDataPath()
            { 
                return Application.persistentDataPath;
            }

            /// <summary>
            /// 获取规范的路径。
            /// </summary>
            /// <param name="path">要规范的路径。</param>
            /// <returns>规范的路径。</returns>
            public static string GetRegularPath(string path)
            {
                if (path == null)
                {
                    return null;
                }

                return path.Replace('\\', '/');
            }

            /// <summary>
            /// 获取连接后的路径。
            /// </summary>
            /// <param name="path">路径片段。</param>
            /// <returns>连接后的路径。</returns>
            public static string GetCombinePath(params string[] path)
            {
                if (path == null || path.Length < 1)
                {
                    return null;
                }

                string combinePath = path[0];
                for (int i = 1; i < path.Length; i++)
                {
                    combinePath = System.IO.Path.Combine(combinePath, path[i]);
                }

                return GetRegularPath(combinePath);
            }

            /// <summary>
            /// 获取远程格式的路径（带有file:// 或 http:// 前缀）。
            /// </summary>
            /// <param name="path">原始路径。</param>
            /// <returns>远程格式路径。</returns>
            public static string GetRemotePath(params string[] path)
            {
                string combinePath = GetCombinePath(path);
                if (combinePath == null)
                {
                    return null;
                }

                return combinePath.Contains("://") ? combinePath : "file://" + combinePath;
            }

            /// <summary>
            /// 获取带有后缀的资源名。
            /// </summary>
            /// <param name="resourceName">原始资源名。</param>
            /// <returns>带有后缀的资源名。</returns>
            public static string GetResourceNameWithSuffix(string resourceName)
            {
                if (string.IsNullOrEmpty(resourceName))
                {
                    throw new Exception("Resource name is invalid.");
                }

                return string.Format("{0}.dat", resourceName);
            }

            /// <summary>
            /// 获取带有 CRC32 和后缀的资源名。
            /// </summary>
            /// <param name="resourceName">原始资源名。</param>
            /// <param name="hashCode">CRC32 哈希值。</param>
            /// <returns>带有 CRC32 和后缀的资源名。</returns>
            public static string GetResourceNameWithCrc32AndSuffix(string resourceName, int hashCode)
            {
                if (string.IsNullOrEmpty(resourceName))
                {
                    throw new Exception("Resource name is invalid.");
                }

                return string.Format("{0}.{1:x8}.dat", resourceName, hashCode);
            }

            /// <summary>
            /// 移除空文件夹。
            /// </summary>
            /// <param name="directoryName">要处理的文件夹名称。</param>
            /// <returns>是否移除空文件夹成功。</returns>
            public static bool RemoveEmptyDirectory(string directoryName)
            {
                if (string.IsNullOrEmpty(directoryName))
                {
                    throw new Exception("Directory name is invalid.");
                }

                try
                {
                    if (!Directory.Exists(directoryName))
                    {
                        return false;
                    }

                    // 不使用 SearchOption.AllDirectories，以便于在可能产生异常的环境下删除尽可能多的目录
                    string[] subDirectoryNames = Directory.GetDirectories(directoryName, "*");
                    int subDirectoryCount = subDirectoryNames.Length;
                    foreach (string subDirectoryName in subDirectoryNames)
                    {
                        if (RemoveEmptyDirectory(subDirectoryName))
                        {
                            subDirectoryCount--;
                        }
                    }

                    if (subDirectoryCount > 0)
                    {
                        return false;
                    }

                    if (Directory.GetFiles(directoryName, "*").Length > 0)
                    {
                        return false;
                    }

                    Directory.Delete(directoryName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}