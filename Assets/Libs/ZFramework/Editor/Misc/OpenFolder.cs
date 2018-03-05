 
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace ZFramework.Editor
{
    /// <summary>
    /// 打开文件夹相关的实用函数。
    /// </summary>
    internal static class OpenFolder
    {
        /// <summary>
        /// 打开 Temporary Cache Path 文件夹。
        /// </summary>
        [MenuItem("ZFramework/Open Folder/temporaryCachePath", false, 13)]
        private static void OpenFolderTemporaryCachePath()
        {
            InternalOpenFolder(Application.temporaryCachePath);
        }

        /// <summary>
        /// 打开 Persistent Data Path 文件夹。
        /// </summary>
        [MenuItem("ZFramework/Open Folder/persistentDataPath", false, 9)]
        private static void OpenFolderPersistentDataPath()
        {
            InternalOpenFolder(Application.persistentDataPath);
        }

        /// <summary>
        /// 打开 Streaming Assets Path 文件夹。
        /// </summary>
        [MenuItem("ZFramework/Open Folder/streamingAssetsPath", false, 11)]
        private static void OpenFolderStreamingAssetsPath()
        {
            InternalOpenFolder(Application.streamingAssetsPath);
        }

        /// <summary>
        /// 打开 Data Path 文件夹。
        /// </summary>
        [MenuItem("ZFramework/Open Folder/dataPath", false, 12)]
        private static void OpenFolderDataPath()
        {
            InternalOpenFolder(Application.dataPath);
        }

        private static void InternalOpenFolder(string folder)
        {
            folder = string.Format("\"{0}\"", folder);
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;
                default:
                    break;
            }
        }
    }
}
